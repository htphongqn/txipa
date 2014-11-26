<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="product.aspx.cs" Inherits="admin_products_product" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="css" Runat="Server">
    <style type="text/css">
        div.row-element fieldset
        {
            min-height: 200px;
        }
    </style>
    <style type="text/css">
       .ToolBar
        {
	        border:solid 1px #d4d4d4;
	        padding:10px;
	        margin-bottom:20px;
        }

        .GridContainer
        {
	        background:#ECF5FB;
	        min-height:300px;
	        border:solid 1px #d4d4d4;
        }


        .ModalPopupBG
        {
	        background-color: #666699;
	        filter: alpha(opacity=50);
	        opacity: 0.7;
        }

        .popup_Container {
	        background-color:#fffeb2;
	        border:2px solid #000000;
	        padding: 0px 0px 0px 0px;
        }

        .popupConfirmation
        {
	       /* width: 300px;
	        height: 200px;*/
        }

        .popup_Titlebar {
	        background: url(../../resources/images/titlebar_bg.jpg);
	        height: 29px;
        }

        .popup_Body
        {
	        padding:15px 15px 15px 15px;
	        font-family:Arial;
	        font-weight:bold;
	        font-size:12px;
	        color:#000000;
	        line-height:15pt;
	        clear:both;
	        padding:20px;
        }

        .TitlebarLeft 
        {
	        float:left;
	        padding-left:5px;
	        padding-top:5px;
	        font-family:Arial, Helvetica, sans-serif;
	        font-weight:bold;
	        font-size:12px;
	        color:#FFFFFF;
        }
        .TitlebarRight 
        {
	        background:url(../../resources/images/cross_icon_normal.png);
	        background-position:right;
	        background-repeat:no-repeat;
	        height:15px;
	        width:16px;
	        float:right;
	        cursor:pointer;
	        margin-right:5px;
	        margin-top:5px;
        }

        .popup_Buttons
        {
	        margin:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="jscript" Runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".decimal").keypress(function (e) {
                if (e.keyCode == 8 || e.keyCode == 46) return true;
                var val = $(this).val();
                var regex = /^(\+|-)?(\d*\.?\d*)$/;
                if (regex.test(val + String.fromCharCode(e.charCode))) {
                    return true;
                }
                return false;
            });
            $(".integer").keypress(function (e) {
                if (e.keyCode == 8 || e.keyCode == 46) return true;
                var val = $(this).val();
                var regex = /^(\+|-)?(\d*)$/;
                if (regex.test(val + String.fromCharCode(e.charCode))) {
                    return true;
                }
                return false;
            });
            $('#<%= ddlProductType.ClientID %>').change(function (e) {
                if ($(this).val() == 1) {
                    $('#contactlenstype').show();
                } else {
                    $('#contactlenstype').hide();
                }
            });

            if ($('#<%= ddlProductType.ClientID %>').val() == 1) {
                $('#contactlenstype').show();
            } else {
                $('#contactlenstype').hide();
            }
        });
    </script>

    <script language="javascript" type="text/javascript">
        function cancel() {
            $get('btnCancel').click();
            $get('btnBrandCancel').click();
            $get('btnCancelCollection').click();
        }
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    
    <h1>
        Product</h1>
<div class="table">
    <div class="row-element">
        <div class="element-header">
                Name</div>
        <div class="element-content">
            <asp:TextBox ID="txtName" runat="server" Width="99%"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="The name field is required!" ControlToValidate="txtName" 
                    ForeColor="#FF3300" ></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row-element">
        <div class="element-header">
                &nbsp;</div>
        <div class="element-content half">
            <asp:CheckBox ID="chkAcvice" runat="server" Text="Active" Checked="True" />
        </div>
        <div class="element-content half">
            <asp:CheckBox ID="chkIsTaxable" runat="server" Text="Taxable" Checked="True" Visible="false"/>
        </div>
        <div class="element-content half">
            <asp:CheckBox ID="chkBestSeller" runat="server" Text="Best Seller" />
        </div>
        <div class="element-content half">
            <asp:CheckBox ID="chkAutoRefill" runat="server" Text="Auto Refill" Visible="false"
                    Checked="True" />
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="row-element">
        <div class="element-header">
                Manufacture</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlManufacture" runat="server" Width="80%" DataTextField="Name" 
                    DataValueField="ManufactureId" AutoPostBack="True" 
                    OnSelectedIndexChanged="ddlManufacture_SelectedIndexChanged" >
            </asp:DropDownList>
            <asp:Button ID="btnAddManu" runat="server" Text="Add" CausesValidation="False"/>
            <cc1:modalpopupextender ID="ModalPopupExtenderManufacture" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancel" TargetControlID="btnAddManu"
                        PopupControlID="Panel1" Drag="true"  PopupDragHandleControlID="PopupHeaderManu">
            </cc1:modalpopupextender>
            <div id="Panel1" style="display: none;" class="popupConfirmation">
                            <%--<iframe id="frameeditexpanse" frameborder="0" src="AddManufacture.aspx" height="200" width="350"
                            scrolling="no"></iframe>--%>
                            <div class="popup_Container">
                                <div class="popup_Titlebar" id="PopupHeaderManu">
                                    <div class="TitlebarLeft">
                                        Add Manufacture
                                    </div>
                                    <div class="TitlebarRight" onclick="cancel();">
                                    </div>
                                </div>
                                <div class="popup_Body">
                                    <%--<asp:MultiView ID="MultiViewExpanse" runat="server">
                                        <asp:View ID="ViewInput" runat="server">--%>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtManuName" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                            ErrorMessage="*" ControlToValidate="txtManuName" ForeColor="Red" 
                                                            ValidationGroup="manu"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Company Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                            ErrorMessage="*" ControlToValidate="txtCompanyName" ForeColor="Red" 
                                                            ValidationGroup="manu"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                    </table>
                                        <%--</asp:View>
                                    </asp:MultiView>--%>
                                </div>
                                <div class="popup_Buttons">
                                    <asp:Button ID="btnOkay" Text="Add" runat="server" OnClick="btnOkay_Click" 
                                        ValidationGroup="manu" />
                                    <input id="btnCancel" value="Cancel" type="button" />
                                </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row-element">
        <div class="element-header">
                Brand</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlBrand" runat="server" Width="80%" DataTextField="Name" 
                    DataValueField="AutoId" AutoPostBack="True" 
                    onselectedindexchanged="ddlBrand_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Button ID="btnAddBrand" runat="server" Text="Add" 
                    CausesValidation="False" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderBrand" runat="server" BackgroundCssClass="ModalPopupBG"
                    TargetControlID="btnAddBrand" PopupControlID="Panel2" CancelControlID="btnBrandCancel" Drag="true" PopupDragHandleControlID="PopupHeaderbrand">
            </cc1:ModalPopupExtender>
            <cc1:DropDownExtender ID="DropDownExtender1" runat="server" TargetControlID="ddlBrand">
            </cc1:DropDownExtender>
            <div id="Panel2" style="display: none;" class="popupConfirmation">
                <div class="popup_Container">
                    <div class="popup_Titlebar" id="PopupHeaderbrand">
                        <div class="TitlebarLeft">
                                Add Brand
                            </div>
                        <div class="TitlebarRight" onclick="cancel();">
                        </div>
                    </div>
                    <div class="popup_Body">
                        <table>
                            <tr>
                                <td>
                                                Name:
                                            </td>
                                <td>
                                    <asp:TextBox ID="txtBrandName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                    ErrorMessage="*" ControlToValidate="txtBrandName" ForeColor="Red" 
                                                    ValidationGroup="brand"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                                Manufacture:
                                            </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlManuBrand" Width="288px"
                                                    DataTextField="Name" DataValueField="ManufactureId">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                                IsPopular:
                                            </td>
                                <td>
                                    <asp:CheckBox ID="chkIsPopular" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                                <%--Image:--%>
                                            </td>
                                <td>
                                    <asp:FileUpload ID="FileUploadBrand" runat="server" Width="60%" 
                                                    Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="popup_Buttons">
                        <asp:Button ID="btnBrandAdd" Text="Add" runat="server" 
                                OnClick="btnBrandAdd_Click" ValidationGroup="brand" />
                        <input id="btnBrandCancel" value="Cancel" type="button" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row-element">
        <div class="element-header">
                Collection</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlCollection" runat="server" Width="80%" DataTextField="Name" DataValueField="AutoId">
            </asp:DropDownList>
            <asp:Button ID="btnAddCollection" runat="server" Text="Add" CausesValidation="False"/>
            <cc1:modalpopupextender ID="ModalPopupExtenderCollection" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancelCollection" TargetControlID="btnAddCollection"
                        PopupControlID="Panel3" Drag="true" 
                    PopupDragHandleControlID="PopupHeaderCollection">
            </cc1:modalpopupextender>
            <div id="Panel3" style="display: none;" class="popupConfirmation">
                   <%-- <iframe id="Iframe2" frameborder="0" src="AddCollection.aspx" height="250" width="450"
                            scrolling="no"></iframe>--%>
                    <div class="popup_Container">
                        <div class="popup_Titlebar" id="PopupHeaderCollection">
                            <div class="TitlebarLeft">
                                Add Collection
                            </div>
                            <div class="TitlebarRight" onclick="cancel();">
                            </div>
                        </div>
                        <div class="popup_Body">
                            <table>
                                <tr>
                                    <td>
                                                Name:
                                            </td>
                                    <td>
                                        <asp:TextBox ID="txtCollectionName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                    ErrorMessage="*" ControlToValidate="txtCollectionName" ForeColor="Red" ValidationGroup="Collection"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                                Manufacture:
                                            </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlManuCollection" Width="288px"
                                                    DataTextField="Name" DataValueField="ManufactureId"
                                                    onselectedindexchanged="ddlManuCollection_SelectedIndexChanged" 
                                                    AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                                    ErrorMessage="*" ControlToValidate="ddlManufacture" ForeColor="Red" ValidationGroup="Collection"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                                Brands:
                                            </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBrandCollection" runat="server" DataTextField="Name" 
                                                DataValueField="AutoId" Width="288px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                                    ErrorMessage="*" ControlToValidate="ddlBrandCollection" ForeColor="Red" ValidationGroup="Collection"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="popup_Buttons">
                            <asp:Button ID="btnCollectionAdd" Text="Add" runat="server" 
                                OnClick="btnCollectionAdd_Click" ValidationGroup="Collection" />
                            <input id="btnCancelCollection" value="Cancel" type="button" />
                        </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row-element">
        <div class="element-header">
                Product Type</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlProductType" runat="server" Width="99%" DataTextField="Name" DataValueField="AutoId">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-element full" style="padding-bottom:0px;">
    </div>
    <div id="contactlenstype" class="row-element" style="display:none">
        <div class="element-header">
                Contact Lens Type</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlContactLensTypes" runat="server" Width="99%" DataTextField="Name" DataValueField="ContactLenseTypeId">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-element">
        <div class="element-header">
                Product State</div>
        <div class="element-content">
            <asp:DropDownList ID="ddlProductState" runat="server" Width="99%" DataTextField="Name" DataValueField="AutoId">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-element full" style="padding-top:20px">
        <div class="element-header">
                Notes</div>
        <div class="element-content">
            <asp:TextBox ID="txtNotes" Rows="4" TextMode="MultiLine" runat="server" Width="99%"></asp:TextBox>
        </div>
    </div>
    <div class="row-element full">
        <fieldset>
            <legend>Picture 2D</legend>
            <div class="element-content half">
                <div class="element-header">
                        Front</div>
                <div class="element-content">
                    <asp:FileUpload ID="filePictureNormal" runat="server" />
                    <br />
                    <asp:Image ID="imgPictureNormal" runat="server" CssClass="smallView" />
                </div>
            </div>
            <div class="element-content half">
                <div class="element-header">
                        Angle</div>
                <div class="element-content">
                    <asp:FileUpload ID="filePictureChoose" runat="server" />
                    <br />
                    <asp:Image ID="imgPictureChoose" runat="server" CssClass="smallView" />
                </div>
            </div>
            <div id="tryonupload" class="element-content half">
                <div class="element-header">
                            Try On</div>
                <div class="element-content">
                    <asp:FileUpload ID="filePictureTryOn" runat="server" />
                    <br />
                    <asp:Image ID="imgPictureTryOn" runat="server" CssClass="smallView" />
                </div>
            </div>
        </fieldset>
    </div>
</div>
<div class="clear">
</div>
<div class="command">
    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click">
    </asp:Button>
    <asp:Button ID="btnCancel2" runat="server" Text="Cancel" CausesValidation="false" onclick="btnCancel_Click">
    </asp:Button>
    <asp:Button ID="btnProductInfo" runat="server" Text="Product Info" >
    </asp:Button>
    <asp:Button ID="btnDetect" runat="server" Text="Detect Image EyeBox" Visible="false"
                    onclick="btnDetect_Click" />
</div>


</asp:Content>