<style>
    @media only screen and (max-width: 1440px) {
        .dvSplitBill {
            top: 5% !important;
            left: 15% !important;
        }
    }

    @media only screen and (max-width: 1340px) {
        .dvSplitBill {
            top: 20% !important;
            left: 12% !important;
        }
    }

    @media only screen and (max-width: 1248px) {
        .dvSplitBill {
            top: 18% !important;
            left: 10% !important;
        }
    }
</style>

<div id="UserValidation" style="max-width: 400px; display: none; z-index: 5000; top: 30%; left: 35%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12">
            <div class="text">
                User Authentication
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        User ID
                    </label>
                    <input type="text" id="txtUserID" autocorrect="off" runat="server" class=" form-control" autocomplete="off" value="" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        User Password
                    </label>

                    <input type="password" id="txtUserPass" runat="server" class=" form-control" autocomplete="off" value="" />
                </div>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label id="lblReason">
                        Less Reason
                    </label>
                    <select name="ddlLessReason" id="ddlLessReason" class="form-control">
                        <option value="3">Customer Not satisfied</option>
                    </select>
                    <select name="ddlCancelReason" id="ddlCancelReason" class="form-control" style="display: block;">
                        <option value="3">Cancel Reason 1</option>
                    </select>
                </div>
            </div>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label id="lblItemType">
                        Item Type
                    </label>
                    <select name="ddlItemType" id="ddlItemType" class="form-control">
                        <option value="1" selected="selected">Not Cooked</option>
                        <option value="2">Cooked</option>
                    </select>
                </div>
            </div>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12">

            <div class="col-md-12">

                <div class="col-md-6" style="margin-top: 5px; margin-left: 40px;">
                    <div class="col-md-5 btn-mar">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;"
                            id="Button1" onclick="UserValidationInDataBase();">
                            Proceed</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                            id="Button2" onclick="CancelUserValidation();">
                            Cancel</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-top: 15px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div id="VoidValidation" style="max-width: 400px; display: none; z-index: 5000; top: 60%; left: 35%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label id="lblVoidReason">
                        Void Reason
                    </label>
                    <select name="ddlVoidReason" id="ddlVoidReason" class="form-control">
                        <option value="3">Customer Not satisfied</option>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12">
                <div class="col-md-6" style="margin-top: 5px; margin-left: 40px;">
                    <div class="col-md-5 btn-mar">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;"
                            id="btnVoidTakeawayOrder">
                            Void</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                            id="btnCancelTakeawayOrder">
                            Cancel</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-top: 15px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div id="ItemCommentPopup" style="max-width: 600px; display: none; z-index: 5000; top: 30%; left: 35%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12" style="text-align: center;">
            <label id="ItemCommentPopuplblHeader" style="font-size: large;">
            </label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" style="margin-top: 5px;">
            <div class="col-md-12">
                <label>
                    Comments
                </label>
                <input type="text" id="txtItemCommentPopupComment" autocorrect="off" runat="server" class=" form-control" autocomplete="off" value="" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-2"></div>
            <div class="col-md-4 btn-mar">
                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;" id="btnItemCommentPopupSave">Save</button>
            </div>
            <div class="col-md-4 btn-mar" style="margin-left: 10px;">
                <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;" id="btnItemCommentPopupCancel">Cancel</button>
            </div>
            <div class="col-md-2"></div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div id="dvSplitBill" style="width: 90%; display: none; z-index: 5000; top: 15%; left: 5%; position: absolute; background-color: #c6c6c6; padding: 10px; border: 1px solid #000; min-height: 500px;">
    <div class="row">
        <div class="col-md-12" style="text-align: center;">
            <label id="lblHeaderSplitBill" style="font-size: large;"></label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <table class="table table-striped table-bordered table-hover table-condensed cf">
                <thead class="cf head" style="background-color: #7dab49;">
                    <tr>
                        <th class="numeric table-text-head">Item(s)
                        </th>
                        <th class="table-text-head" style="text-align: center;">Quantity
                        </th>
                        <th class="table-text-head" style="text-align: center;">Split Item
                        </th>
                    </tr>
                </thead>
                <tbody id="tble-split-original">
                </tbody>
            </table>
        </div>
        <div class="col-md-6">
            <table class="table table-striped table-bordered table-hover table-condensed cf">
                <thead class="cf head" style="background-color: #7dab49;">
                    <tr>
                        <th class="numeric table-text-head">Item(s)
                        </th>
                        <th class="table-text-head" style="text-align: center;">Quantity
                        </th>
                        <th class="table-text-head" style="text-align: center;">Remove Item
                        </th>
                    </tr>
                </thead>
                <tbody id="tble-split-order">
                </tbody>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
        </div>
        <div class="col-md-6">
            <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;" id="btnSplitBillSave">Split Order</button>
            <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 5px; font-size: 15px; color: #FFF; background-color: #f94d72;" id="btnSplitBillCancel">Cancel</button>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div id="ComplimentaryValidation" style="max-width: 450px; display: none; z-index: 5000; top: 30%; left: 50%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label id="lblComplimentaryReason">
                        Complimentary Reason
                    </label>
                    <select name="ddlComplimentaryReason" id="ddlComplimentaryReason" class="form-control">
                        <option value="3">Director Complimentary</option>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12">
                <div class="col-md-6" style="margin-top: 5px; margin-left: 40px;">
                    <div class="col-md-5 btn-mar">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;"
                            id="btnComplimentaryItem">
                            Complimentary</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                            id="btnCancelComplimentaryItem">
                            Cancel</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-top: 15px;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>