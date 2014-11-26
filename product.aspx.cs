using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using SmartES.DL;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SmartES.SV;

public partial class admin_products_product : System.Web.UI.Page
{
    private ProductModel model;
    private int id, type;
    private int ProductTypeId;
    private int ThumbHeight = 52, ThumbWidth = 74;
    private int SmallHeight = 249, SmallWidth = 356;
    private int BigHeight = 700, BigWidth = 1000;

    protected void Page_Load ( object sender, EventArgs e )
    {
        model = new ProductModel();
        Initial();
    }

    private void Initial ()
    {
        if ( !IsPostBack )
        {
            ddlManuBrand.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
            ddlManuCollection.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
            ddlManufacture.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
            ddlProductType.DataSource = BaseModel.GetAllByTableName("ProductsType");
            ddlProductState.DataSource = BaseModel.GetAllByTableName("ProductState");
            ddlContactLensTypes.DataSource = BaseModel.GetDataTable("spGetListContactLenseTypes");
            ddlManufacture.DataBind();
            ddlProductType.DataBind();
            ddlProductState.DataBind();
            ddlContactLensTypes.DataBind();
            ddlManuBrand.DataBind();
            ddlManuCollection.DataBind();
            LoadBrandCollection(Convert.ToInt32(ddlManuCollection.SelectedValue));
            Utilities.AddBlankDropdownItem(ddlProductState);

            int.TryParse(Request.Params["id"], out id);
            int.TryParse(Request.Params["type"], out type);

            if ( id > 0 )
            {
                DataRow row = model.GetProductById(id);
                txtName.Text = BaseModel.GetStringFieldValue(row, "Name");

                chkAcvice.Checked = ( row["IsActive"] != DBNull.Value ? Convert.ToBoolean(row["IsActive"]) : false );
                chkBestSeller.Checked = ( row["IsBestSeller"] != DBNull.Value ? Convert.ToBoolean(row["IsBestSeller"]) : false );
                chkIsTaxable.Checked = ( row["IsTax"] != DBNull.Value ? Convert.ToBoolean(row["IsTax"]) : false );
                chkAutoRefill.Checked = ( row["IsAutoRefill"] != DBNull.Value ? Convert.ToBoolean(row["IsAutoRefill"]) : false );

                Utilities.SelectDropdownItem(ddlManufacture, row["ManufactureId"]);
                Utilities.SelectDropdownItem(ddlProductType, row["ProductTypeId"]);
                Utilities.SelectDropdownItem(ddlProductState, row["ProductStateId"]);
                Utilities.SelectDropdownItem(ddlContactLensTypes, row["ContactLensTypeId"]);

                LoadBrands(row["BrandId"]);
                LoadCollections(row["CollectionId"]);

                txtNotes.Text = ( row["Notes"] != DBNull.Value ? Convert.ToString(row["Notes"]) : "" );

                if ( row["PictureNormal"] != DBNull.Value )
                {
                    string fileName = Convert.ToString(row["PictureNormal"]);
                    imgPictureNormal.ImageUrl = Utilities.GetImagePath(Convert.ToInt32(row["ProductTypeId"])) + "/Smalls/S_" + fileName;
                    ViewState["PictureNormal"] = fileName;
                }

                if ( row["PictureChoose"] != DBNull.Value )
                {
                    string fileName = Convert.ToString(row["PictureChoose"]);
                    imgPictureChoose.ImageUrl = Utilities.GetImagePath(Convert.ToInt32(row["ProductTypeId"])) + "/Smalls/S_" + fileName;
                    ViewState["PictureChoose"] = fileName;
                }

                if ( row["PictureTryOn"] != DBNull.Value )
                {
                    string fileName = BaseModel.GetStringFieldValue(row, "PictureTryOn");
                    imgPictureTryOn.ImageUrl = "~/resources/images/TryOn/Glasses/" + fileName;
                    ViewState["PictureTryOn"] = fileName;
                }

                ProductTypeId = BaseModel.GetIntFieldValue(row, "ProductTypeId");
                ViewState["ProductTypeId"] = ProductTypeId;
                if (ProductTypeId == 2)
                {
                    btnDetect.Visible = true;
                }
            }
            else
            {
                btnProductInfo.Visible = false;
                
                if ( type > 0 )
                {
                    Utilities.SelectDropdownItem(ddlProductType, type);
                    ddlProductType.Enabled = false;
                }
                else
                {
                    btnCancel2.Visible = false;
                }

                LoadBrands(0);
            }

            ViewState["ID"] = id;            
        }
        else
        {
            id = Convert.ToInt32(ViewState["ID"] != null ? ViewState["ID"] : 0);
        }

        if ( type == 1 )
        {
            Utilities.AddBlankDropdownItem(ddlCollection);
            Utilities.AddBlankDropdownItem(ddlBrand);
        }

        btnProductInfo.PostBackUrl = "~/admin/products/productinfo.aspx?id=" + id;

    }

    private bool ValidateData ()
    {
        if ( txtName.Text.Trim() == string.Empty )
            return false;

        return true;
    }

    //protected void btnSave_Click ( object sender, EventArgs e )
    //{
    //    if ( ValidateData() )
    //    {
    //        string path = Utilities.GetImagePath(Convert.ToInt32(ddlProductType.SelectedValue));

    //        string filePath = path + "/Bigs/",
    //               fileSmallPath = path + "/Smalls/",
    //               fileThumbPath = path + "/Thumbs/";

    //        string Prefix = Utilities.GetPrefixImage(Convert.ToInt32(ddlProductType.SelectedValue));

    //        string namePictureNormal = ( ViewState["PictureNormal"] != null ? ViewState["PictureNormal"].ToString() : "" );
    //        int NextProductId = (new ProductService()).GetIdentityCurrentForProduct();
    //        if (id > 0)
    //        {
    //            NextProductId = id;
    //            ProductTypeId = ViewState["ProductTypeId"] != null ? Convert.ToInt32(ViewState["ProductTypeId"]) : 0;
    //            if (ProductTypeId != 0 && ProductTypeId != Convert.ToInt32(ddlProductType.SelectedValue))
    //            {
    //                Utilities.MoveImageBetweenTwoProductType(id, ProductTypeId, Convert.ToInt32(ddlProductType.SelectedValue));
    //            }
    //        }

    //        if ( filePictureNormal.HasFile )
    //        {
    //            if ( Utilities.CheckImageFileType(filePictureNormal.FileName) )
    //            {
    //                namePictureNormal = NextProductId.ToString() + "_n" + System.IO.Path.GetExtension(filePictureNormal.FileName);
    //                Utilities.ResizeUploadedImage(filePictureNormal, MapPath(filePath + "B_" + namePictureNormal), BigWidth, BigHeight);
    //                Utilities.ResizeUploadedImage(filePictureNormal, MapPath(fileSmallPath + "S_" + namePictureNormal), SmallWidth, SmallHeight);
    //                Utilities.ResizeUploadedImage(filePictureNormal, MapPath(fileThumbPath + "T_" + namePictureNormal), ThumbWidth, ThumbHeight);
    //            }
    //        }

    //        string namePictureChoose = ( ViewState["PictureChoose"] != null ? ViewState["PictureChoose"].ToString() : "" );
    //        if ( filePictureChoose.HasFile )
    //        {
    //            if ( Utilities.CheckImageFileType(filePictureChoose.FileName) )
    //            {
    //                namePictureChoose = NextProductId.ToString() + "_c" + System.IO.Path.GetExtension(filePictureChoose.FileName);
    //                Utilities.ResizeUploadedImage(filePictureChoose, MapPath(filePath + "B_" + namePictureChoose), BigWidth, BigHeight);
    //                Utilities.ResizeUploadedImage(filePictureChoose, MapPath(fileSmallPath + "S_" + namePictureChoose), SmallWidth, SmallHeight);
    //                Utilities.ResizeUploadedImage(filePictureChoose, MapPath(fileThumbPath + "T_" + namePictureChoose), ThumbWidth, ThumbHeight);
    //            }
    //        }

    //        string namePictureTryOn = (ViewState["PictureTryOn"] != null ? ViewState["PictureTryOn"].ToString() : "");
    //        if (filePictureTryOn.HasFile)
    //        {
    //            if (Utilities.CheckImageFileType(filePictureTryOn.FileName))
    //            {
    //                if (namePictureTryOn == "")
    //                    namePictureTryOn = NextProductId.ToString() + "_t" + System.IO.Path.GetExtension(filePictureTryOn.FileName);
    //                Utilities.ResizeUploadedImage(filePictureTryOn, MapPath("~/resources/images/TryOn/" + ( ProductTypeId == 2? "Glasses" : "Sunglasses") + "/" + namePictureTryOn), 250, 90);
    //            }
    //        }

    //        int productState = ( ddlProductState.SelectedValue != "" ? Convert.ToInt32(ddlProductState.SelectedValue) : 0 );
    //        int productType = ( ddlProductType.SelectedValue != "" ? Convert.ToInt32(ddlProductType.SelectedValue) : 0 );
    //        int collection = ( ddlCollection.SelectedValue != "" ? Convert.ToInt32(ddlCollection.SelectedValue) : 0 );
    //        int contactLensType = ( ddlProductType.SelectedValue == "1" ? Convert.ToInt32(ddlContactLensTypes.SelectedValue) : 0 );

    //        id = model.InsertUpdateProductMainInfo(id, txtName.Text, txtNotes.Text, chkAcvice.Checked, chkBestSeller.Checked, collection, productState, namePictureChoose, namePictureNormal, namePictureTryOn, productType, contactLensType, chkIsTaxable.Checked, chkAutoRefill.Checked);
    //        Response.Redirect("~/admin/products/productinfo.aspx?id=" + id);
    //    }
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ValidateData())
        {
            string path = Utilities.GetImagePath(Convert.ToInt32(ddlProductType.SelectedValue));
            string filePath = path + "/Bigs/",
                   fileSmallPath = path + "/Smalls/",
                   fileThumbPath = path + "/Thumbs/";

            string Name = txtName.Text.Replace(" ", "");
            string Prefix = Utilities.GetPrefixImage(Convert.ToInt32(ddlProductType.SelectedValue));            
            int NextProductId = (new ProductService()).GetIdentityCurrentForProduct();
            if (id > 0)
            {
                NextProductId = id;
                ProductTypeId = ViewState["ProductTypeId"] != null ? Convert.ToInt32(ViewState["ProductTypeId"]) : 0;
                if (ProductTypeId != 0 && ProductTypeId != Convert.ToInt32(ddlProductType.SelectedValue))
                {
                    Utilities.MoveImageBetweenTwoProductType(id, ProductTypeId, Convert.ToInt32(ddlProductType.SelectedValue));
                }
                DataRow row = model.GetProductById(id);
                Name = BaseModel.GetStringFieldValue(row, "Name");
                Name = Name.Replace(" ", "");
            }
            string namePictureNormal = Name + "_F.png";
            if (filePictureNormal.HasFile)
            {
                if (Utilities.CheckImageFileType(filePictureNormal.FileName))
                {
                    namePictureNormal = Name + "_F" + System.IO.Path.GetExtension(filePictureNormal.FileName);
                    Utilities.ResizeUploadedImage(filePictureNormal, MapPath(filePath + "B_" + namePictureNormal), BigWidth, BigHeight);
                    Utilities.ResizeUploadedImage(filePictureNormal, MapPath(fileSmallPath + "S_" + namePictureNormal), SmallWidth, SmallHeight);
                    Utilities.ResizeUploadedImage(filePictureNormal, MapPath(fileThumbPath + "T_" + namePictureNormal), ThumbWidth, ThumbHeight);
                }
            }

            string namePictureChoose = Name + "_A.png";
            if (filePictureChoose.HasFile)
            {
                if (Utilities.CheckImageFileType(filePictureChoose.FileName))
                {
                    namePictureChoose = Name + "_A" + System.IO.Path.GetExtension(filePictureChoose.FileName);
                    Utilities.ResizeUploadedImage(filePictureChoose, MapPath(filePath + "B_" + namePictureChoose), BigWidth, BigHeight);
                    Utilities.ResizeUploadedImage(filePictureChoose, MapPath(fileSmallPath + "S_" + namePictureChoose), SmallWidth, SmallHeight);
                    Utilities.ResizeUploadedImage(filePictureChoose, MapPath(fileThumbPath + "T_" + namePictureChoose), ThumbWidth, ThumbHeight);
                }
            }

            string namePictureTryOn = Name + "_2D.png";
            if (filePictureTryOn.HasFile)
            {
                if (Utilities.CheckImageFileType(filePictureTryOn.FileName))
                {
                    namePictureTryOn = Name + "_2D" + System.IO.Path.GetExtension(filePictureTryOn.FileName);
                    Utilities.ResizeUploadedImage(filePictureTryOn, MapPath("~/resources/images/TryOn/" + (ProductTypeId == 2 ? "Glasses" : "Sunglasses") + "/" + namePictureTryOn), 250, 90);
                }
            }

            int productState = (ddlProductState.SelectedValue != "" ? Convert.ToInt32(ddlProductState.SelectedValue) : 0);
            int productType = (ddlProductType.SelectedValue != "" ? Convert.ToInt32(ddlProductType.SelectedValue) : 0);
            int collection = (ddlCollection.SelectedValue != "" ? Convert.ToInt32(ddlCollection.SelectedValue) : 0);
            int contactLensType = (ddlProductType.SelectedValue == "1" ? Convert.ToInt32(ddlContactLensTypes.SelectedValue) : 0);

            id = model.InsertUpdateProductMainInfo(id, txtName.Text, txtNotes.Text, chkAcvice.Checked, chkBestSeller.Checked, collection, productState, namePictureChoose, namePictureNormal, namePictureTryOn, productType, contactLensType, chkIsTaxable.Checked, chkAutoRefill.Checked);
            Response.Redirect("~/admin/products/productinfo.aspx?id=" + id);
        }
    }
    protected void btnCancel_Click ( object sender, EventArgs e )
    {
        int type = 0;
        int.TryParse(ddlProductType.SelectedValue, out type);
        switch ( type )
        {
            case 1:
                Response.Redirect("contactlens.aspx");
                break;
            case 2:
                Response.Redirect("glasses.aspx");
                break;
            case 3:
                Response.Redirect("sunglasses.aspx");
                break;
            case 4:
                Response.Redirect("accessories.aspx");
                break;
        }        
    }
    protected void ddlManufacture_SelectedIndexChanged ( object sender, EventArgs e )
    {
        LoadBrands(0);
        string i = ddlManufacture.SelectedValue;
        ddlManuBrand.SelectedValue = i.ToString(); 
        ddlManuCollection.SelectedValue = i.ToString(); LoadBrandCollection(Convert.ToInt32(ddlManuCollection.SelectedValue));
    }
    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCollections(0);
    }
    private void LoadBrands (object brandId)
    {
        if ( ddlManufacture.SelectedValue != "" )
        {
            ddlBrand.DataSource = BaseModel.GetDataTable("spGetListBrandsByManufactureId_Admin", Convert.ToInt32(ddlManufacture.SelectedValue), null);
            ddlBrand.DataBind();

            Utilities.SelectDropdownItem(ddlBrand, brandId);
            LoadCollections(0);
        }
        else
        {
            ddlBrand.Items.Clear();
            ddlCollection.Items.Clear();
        }



    }
    private void LoadCollections ( object collectionId )
    {
        if ( ddlBrand.SelectedValue != "" )
        {
            ddlCollection.DataSource = BaseModel.GetDataTable("GetListCollectionByBrandId", Convert.ToInt32(ddlBrand.SelectedValue));
            ddlCollection.DataBind();

            Utilities.SelectDropdownItem(ddlCollection, collectionId);
        }
        else
        {
            ddlCollection.Items.Clear();
        }
    }
    protected void btnLoadManu_Click(object sender, EventArgs e)
    {
        ddlManufacture.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
        ddlManufacture.DataBind();
    }
    protected void btnLoadBrand_Click(object sender, EventArgs e)
    {
        ddlManufacture_SelectedIndexChanged(sender, e);
    }
    protected void btnLoadCollection_Click(object sender, EventArgs e)
    {
        ddlBrand_SelectedIndexChanged(sender, e);
    }
    protected void btnDetect_Click(object sender, EventArgs e)
    {
        int ThumbHeight = 52, ThumbWidth = 74;
        int SmallHeight = 249, SmallWidth = 356;
        int BigHeight = 700, BigWidth = 1000;
        int TryonHeight = 90, TryOnWidth = 250;
        int ProductTypeId = 2;
        string path = Utilities.GetImagePath(ProductTypeId);
        string filePath = path + "/Bigs/",
                fileSmallPath = path + "/Smalls/",
                fileThumbPath = path + "/Thumbs/";
        string EyeBoxPath = "~/EyeBoxImage/";
        string EyeBoxCompletePath = "~/EyeBoxImage/EyeBoxImageComplete/";
        DirectoryInfo drInfo = new DirectoryInfo(Server.MapPath(EyeBoxPath));
        FileInfo[] fInfo = drInfo.GetFiles();
        if (fInfo.Length > 0)
        {
            foreach (FileInfo item in fInfo)
            {
                string strName = item.Name.Replace(" ", "");
                string typeName = strName.Substring(strName.LastIndexOf("_") + 1);
                FileStream stream = new FileStream(MapPath(EyeBoxPath + item.Name), FileMode.Open);
                switch (typeName)
                {
                    case "F.png":

                        Utilities.ResizeUploadedImage(stream, MapPath(filePath + "B_" + strName), BigWidth, BigHeight);
                        Utilities.ResizeUploadedImage(stream, MapPath(fileSmallPath + "S_" + strName), SmallWidth, SmallHeight);
                        Utilities.ResizeUploadedImage(stream, MapPath(fileThumbPath + "T_" + strName), ThumbWidth, ThumbHeight);
                        break;
                    case "A.png":
                        Utilities.ResizeUploadedImage(stream, MapPath(filePath + "B_" + strName), BigWidth, BigHeight);
                        Utilities.ResizeUploadedImage(stream, MapPath(fileSmallPath + "S_" + strName), SmallWidth, SmallHeight);
                        Utilities.ResizeUploadedImage(stream, MapPath(fileThumbPath + "T_" + strName), ThumbWidth, ThumbHeight);
                        break;
                    case "2D.png":
                        Utilities.ResizeUploadedImage(stream, MapPath("~/resources/images/TryOn/" + (ProductTypeId == 2 ? "Glasses" : "Sunglasses") + "/" + strName), TryOnWidth, TryonHeight);
                        break;
                    default:
                        break;
                }

                stream.Dispose();
                if (File.Exists(Server.MapPath(EyeBoxCompletePath + strName)))
                {
                    File.Delete(Server.MapPath(EyeBoxCompletePath + strName));
                }
                item.MoveTo(Server.MapPath(EyeBoxCompletePath + strName));
                //Response.Write("<script>alert('Successfull !');</script>");
            }
        }
        else
        {
            //Response.Write("<script>alert('No Image Detected !');</script>");
        }
    }
    protected void btnDetectFrameImage3D_Click(object sender, EventArgs e)
    {
        int ProductTypeId = 2;
        string path = Utilities.GetImagePath(ProductTypeId);
        string filePath = path + "/Bigs/",
                fileSmallPath = path + "/Smalls/",
                fileThumbPath = path + "/Thumbs/";
        string EyeBoxPath = "~/EyeBoxImage3D/";
        string EyeBoxCompletePath = "~/EyeBoxImage3D/EyeBoxImageComplete3D/";
        DirectoryInfo drInfo = new DirectoryInfo(Server.MapPath(EyeBoxPath));
        FileInfo[] fInfo = drInfo.GetFiles();
        if (fInfo.Length > 0)
        {
            foreach (FileInfo item in fInfo)
            {
                string strName = item.Name.Replace(" ", "");
                string typeName = strName.Substring(strName.LastIndexOf("_") + 1);
                FileStream stream = new FileStream(MapPath(EyeBoxPath + item.Name), FileMode.Open);
                switch (typeName)
                {
                    case "C.png":                        
                        Utilities.UploadedFrame3D(stream, MapPath("~/resources/images/TryOn/" + (ProductTypeId == 2 ? "Glasses" : "Sunglasses") + "/" + strName));
                        break;
                    case "L.png":
                        Utilities.UploadedFrame3D(stream, MapPath("~/resources/images/TryOn/" + (ProductTypeId == 2 ? "Glasses" : "Sunglasses") + "/" + strName));
                        break;
                    case "R.png":
                        Utilities.UploadedFrame3D(stream, MapPath("~/resources/images/TryOn/" + (ProductTypeId == 2 ? "Glasses" : "Sunglasses") + "/" + strName));
                        break;
                    default:
                        break;
                }

                stream.Dispose();
                if (File.Exists(Server.MapPath(EyeBoxCompletePath + strName)))
                {
                    File.Delete(Server.MapPath(EyeBoxCompletePath + strName));
                }
                item.MoveTo(Server.MapPath(EyeBoxCompletePath + strName));
                //Response.Write("<script>alert('Successfull !');</script>");
            }
        }
        else
        {
            //Response.Write("<script>alert('No Image Detected !');</script>");
        }
    }

    //pupop show
    protected void btnOkay_Click(object sender, EventArgs e)
    {
        try
        {
            int i = new ManufacturerModel().InsertUpdateManufacturer(0, txtManuName.Text, txtCompanyName.Text);
            ddlManufacture.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
            ddlManufacture.DataBind();
            ddlManufacture.SelectedValue = i.ToString();
            LoadBrands(i);

            ddlManuBrand.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin"); ddlManuBrand.DataBind();
            ddlManuCollection.DataSource = BaseModel.GetDataTable("spGetListManufactures_Admin");
            ddlManuCollection.DataBind();
            ddlManuBrand.SelectedValue = i.ToString(); ddlManuCollection.SelectedValue = i.ToString();
            LoadBrandCollection(Convert.ToInt32(ddlManuCollection.SelectedValue));

            txtManuName.Text = "";
            txtCompanyName.Text = "";
        }
        catch
        {
        }
    }
    protected void btnBrandAdd_Click(object sender, EventArgs e)
    {
        string filePath = "/resources/images/brands/";
        string fileName = "";
        int ManufactureId = Convert.ToInt32(ddlManuBrand.SelectedValue);

        if (FileUploadBrand.HasFile)
        {
            if (Utilities.CheckImageFileType(FileUploadBrand.FileName))
            {
                fileName = DateTime.Now.ToString("yyyyMMddhhttss") + System.IO.Path.GetExtension(FileUploadBrand.FileName);
                FileUploadBrand.SaveAs(MapPath("~" + filePath + fileName));
            }
        }

        int i = new BrandModel().InsertUpdateBrand(0, txtBrandName.Text, fileName, ManufactureId, false, false, false, false, chkIsPopular.Checked);

        LoadBrands(i);
        txtBrandName.Text = "";
        LoadBrandCollection(Convert.ToInt32(ddlManuCollection.SelectedValue));
        chkIsPopular.Checked = false;
        
    }
    protected void btnCollectionAdd_Click(object sender, EventArgs e)
    {
        int i = new BrandModel().InsertUpdate_Collections(0, txtCollectionName.Text.Trim(), Convert.ToInt32(ddlBrandCollection.SelectedValue), true);
        LoadCollections(i);
        txtCollectionName.Text = "";        
    }
    private void LoadBrandCollection(object manufactureId)
    {
        ddlBrandCollection.DataSource = BaseModel.GetDataTable("spGetListBrandsByManufactureId_Admin", manufactureId, null);
        ddlBrandCollection.DataBind();
    }
    protected void ddlManuCollection_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadBrandCollection(Convert.ToInt32(ddlManuCollection.SelectedValue));
        ModalPopupExtenderCollection.Show();
    }
}