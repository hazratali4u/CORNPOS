<style>
    .ui-widget.ui-widget-content {
    border: 1px solid #c5c5c5;
    z-index: 9999;
}
</style>
<div id="dvOpenItem" style="max-width: 400px; display: none; z-index: 5000; top: 10%; left: 35%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12">
            <div class="text">
                Open Item
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Section
                    </label>
                    <select name="ddlSectionOpenItem" id="ddlSectionOpenItem" class="form-control">
                        <option value="1">Section 1</option>
                    </select>
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Category
                    </label>
                    <select name="ddlCategoryOpenItem" id="ddlCategoryOpenItem" class="form-control">
                        <option value="1">Category 1</option>
                    </select>
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Item Name
                    </label>
                    <input type="text" id="txtOpentItemName" runat="server" class=" form-control" value="" />
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Quantity
                    </label>
                    <input type="text" id="txtQuantityOpenItem" runat="server" class=" form-control" value="" onkeypress="return onlyDotsAndNumbers(this,event);"/>
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Price
                    </label>
                    <input type="text" id="txtPriceOpenItem" runat="server" class=" form-control" value="" onkeypress="return onlyDotsAndNumbers(this,event);"/>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-6" style="margin-top: 5px; margin-left: 40px;">
                    <div class="col-md-5 btn-mar">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;"
                            id="btnSaveOpenItem" onclick="AddOpenItem();">
                            Add</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                            id="btnCancelSaveOpenItem" onclick="CloseOpenItemPopup();">
                            Cancel</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-top: 15px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>