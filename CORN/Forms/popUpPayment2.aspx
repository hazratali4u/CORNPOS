<style>
    @media only screen and (max-width: 1440px) {
        .paymentpop2 {
            top: 5% !important;
            left: 15% !important;
        }
    }

    @media only screen and (max-width: 1340px) {
        .paymentpop2 {
            top: 20% !important;
            left: 12% !important;
        }
    }

    @media only screen and (max-width: 1248px) {
        .paymentpop2 {
            top: 18% !important;
            left: 10% !important;
        }
    }
</style>
<div class="paymentpop2" id="payment2" style="max-width: 1000px; display: none; top: 15%; left: 20%; position: absolute; background-color: #c6c6c6; padding: 10px; border: 1px solid #000; min-height: 400px;">
    <div class="row" style="margin-left:0px;">
        <div class="col-md-2" style="border-right: 1px solid #dadada; padding-top: 25px;">
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="cash2" value="0">
                Cash
            </button>
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="credit2"
                value="1">
                Credit Card
            </button>
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="btnCredit2"
                value="2">
                Credit 
            </button>
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="btnEasypaisa2"
                value="3">
                Easypaisa
            </button>
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="btnJazzcash2"
                value="4">
                Jazz Cash
            </button>
            <button type="button" class="btn btn-toolbar" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #919399; margin-bottom: 10px;"
                onclick="PayType2(this)" id="btnOnlineTransfer2"
                value="5">
                Online Tran
            </button>
        </div>
        <div class="col-md-4">
            <div class="text">
                Payment Mode
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; padding-left: 5px;">
                        <label id="subTotal2" style="font: bold 32px sans-serif; display: none; color: #fff; background-color: #000; text-align: right; display: none;">
                            0.00
                        </label>
                        <label id="SalesTax2" style="font: bold 18px sans-serif; display: none; color: #fff; background-color: #000; text-align: right; display: none;">
                            0.00
                        </label>
                        <label style="font: bold 17px sans-serif;">
                            Gross Amount
                        </label>
                    </div>
                    <div class="col-md-4" style="text-align: right;">
                        <label runat="server" id="GrandTotal2" style="font: bold 32px sans-serif;">
                            0.00
                        </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; padding-left: 5px;">
                        <label style="font: bold 17px sans-serif;">
                            Discount Amount
                        </label>
                    </div>
                    <div class="col-md-4" style="text-align: right;">
                        <label runat="server" id="lblDiscountTotal2" style="font: bold 32px sans-serif;">
                            0.00
                        </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; padding-left: 5px;">
                        <label runat="server" id="lblGSTOrService2" style="font: bold 17px sans-serif;">
                            GST Amount
                        </label>
                    </div>
                    <div class="col-md-4" style="text-align: right;">
                        <label runat="server" id="lblGSTTotal2" style="font: bold 32px sans-serif; cursor: pointer;" ondblclick="VoidGST();">
                            0.00
                        </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; padding-left: 5px;" id="divServiceCharges">
                        <label runat="server" id="lblServiceCharges2" style="font: bold 17px sans-serif;">
                            Service Charges
                        </label>
                    </div>
                    <div class="col-md-4" id="divServiceCharges2" style="text-align: right;">
                        <label runat="server" id="lblServiceChargesTotal" style="font: bold 32px sans-serif;">
                            0
                        </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; padding-left: 5px;">
                        <label style="font: bold 17px sans-serif;">
                            Payment Due</label>
                    </div>
                    <div class="col-md-4" style="text-align: right;">
                        <label runat="server" id="lblPaymentDue2" style="font: bold 32px sans-serif;">
                            0.00
                        </label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; display: none; padding-left: 5px;" id="dvAdvanceLable2">
                        <label style="font: bold 17px sans-serif; color: red">Advance</label>
                    </div>
                    <div class="col-md-4" id="dvAdvanceAmount2" style="display: none; text-align: right">
                        <label runat="server" id="lblAdvance2" style="font: bold 32px sans-serif; color: red;">0.00</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-8" style="margin-top: 7px; display: none; padding-left: 5px;" id="dvBalanceLable2">
                        <label style="font: bold 17px sans-serif; color: red;">Receivables</label>
                    </div>
                    <div class="col-md-4" id="dvBalanceAmount2" style="display: none; text-align: match-parent;">
                        <label runat="server" id="lblBalanceAmount2" style="font: bold 32px sans-serif; color: red;">0.00</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="border-left: 1px solid #dadada;" id="divPaymentTab2">
            <div class="row" id="dvDiscount2">
                <div class="text">Discount Type</div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #919399;"
                        onclick="DiscType(this);" id="percentage2"
                        value="0">
                        %age</button>
                </div>
                <div class="col-md-1">
                </div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #919399;"
                        onclick="DiscType(this)" id="value2"
                        value="1">
                        Value</button>
                </div>
                <div class="col-md-1">
                </div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: rgb(255, 255, 255); background-color: rgb(125, 171, 73);" onclick="DiscountAuthentication()" id="btnDiscountAuthentication" value="1">Authentic Discount</button>
                </div>
            </div>
            <div class="row" style="margin-top: 5px;">
                <div class="col-md-4">
                    <label>
                        Discount</label>
                    <input type="text" id="txtDiscount2" runat="server" class="form-control" style="font-size: 20px; font-weight: bold;"
                        onkeyup="CalculateBalance2();" value="0" onkeypress="return onlyDotsAndNumbers(this,event);"
                        onclick="ShowCustomKeyBoadDecimal(this)" disabled="disabled" />
                </div>
                <div class="col-md-8">
                    <label>
                        Discount/Complimentary Reason
                    </label>
                    <input type="text" id="txtDiscountReason2" runat="server" class="form-control" />
                </div>
            </div>
            <div class="row" style="margin-top: 7px;" runat="server">
                <div class="col-md-12">
                    <label>
                        Disc Type
                    </label>
                    <select id="ddlDiscountType2" onchange="loadUsers2(this);" class="form-control" style="width: 100%;">
                        <option value="-1">--Select--</option>
                        <option value="0">General</option>
                        <option value="1">Employee Discount</option>
                        <option value="2">Loyalty Card</option>
                    </select>
                </div>
            </div>
            <div class="row" style="display: none;" id="dvBankDiscount2">
                <fieldset>
                    <div class="col-md-4">
                        <label>Bank Discount</label>
                        <select id="ddlBankDiscount2" class="form-control" onchange="ApplyBankDiscount2();" style="width: 100%;"></select>
                    </div>
                    <div class="col-md-4">
                        <label>Credit Card No</label>
                        <input type="text" id="txtCreditCardNo2" class="txtBox form-control" />
                    </div>
                    <div class="col-md-4">
                        <label>Account Tile</label>
                        <input type="text" id="txtCreditCardAccountTile2" class="txtBox form-control" />
                    </div>
                </fieldset>
            </div>
            <div class="row" style="display: none;" id="dvDiscountUser2">
                <fieldset>
                    <div class="col-md-9">
                        <label>
                            Employee Name
                        </label>
                        <select id="ddlDiscountUser4" class="form-control" onchange="loadLimit2(this);"></select>
                    </div>
                    <div class="col-md-3">
                        <label>Limit </label>
                        <label id="lblLimit2" style="font-size: 14px"></label>
                    </div>
                </fieldset>
            </div>
            <div class="row" style="display: none;" id="dvLoyaltyCard2">
                <fieldset>
                    <div class="row">
                        <div class="col-md-6" style="padding-left: 5px; padding-right: 5px;">
                            <label>Card No</label>
                            <input type="text" id="txtLoyaltyCard2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;"
                                onblur="LoadLoyaltyCardDetail2();" />
                        </div>
                        <div class="col-md-6" style="padding-left: 5px; padding-right: 5px;">
                            <label>Customer Name</label>
                            <input type="text" id="txtLoyaltyCustomer2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                    </div>
                    <div class="row" id="rowPrivilege2" style="display: none;">
                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>No Of Visits</label>
                            <input type="text" id="txtNoOfVisits2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Total Purchased</label>
                            <input type="text" id="txtTotalPurchased2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>

                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Total Discount</label>
                            <input type="text" id="txtTotalLoyaltyDiscount2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Purchased Qty</label>
                            <input type="text" id="txtLoyaltyQuantity2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                    </div>
                    <div class="row" id="rowDirectorCard2" style="display: none;">
                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Allowed Limit</label>
                            <input type="text" id="txtAllowedLimit2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Availed Discount</label>
                            <input type="text" id="txtDiscountAvail2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>

                        <div class="col-md-3" style="padding-left: 5px; padding-right: 5px;">
                            <label>Balance Discount</label>
                            <input type="text" id="txtDiscountBalance2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                    </div>
                    <div class="row" id="rowRewardCard2" style="display: none;">
                        <div class="col-md-2" style="padding-left: 5px; padding-right: 5px;">
                            <label>Total Points</label>
                            <input type="text" id="txtTotalPoints2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-2" style="padding-left: 5px; padding-right: 5px;">
                            <label>Red. Points</label>
                            <input type="text" id="txtRedeemedPoints2" class="txtBox form-control" onblur="CalculateDiscountOnRedemption();" style="font-size: 20px; font-weight: bold;" />
                        </div>

                        <div class="col-md-2" style="padding-left: 5px; padding-right: 5px;">
                            <label>Bal. Points</label>
                            <input type="text" id="txtBalancePoints2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-2" style="padding-left: 5px; padding-right: 5px; display: none;">
                            <label>Avail. Disc.</label>
                            <input type="text" id="txtAvailableDiscount2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                        <div class="col-md-2" style="padding-left: 5px; padding-right: 5px; display: none;">
                            <label>Avail. Cash</label>
                            <input type="text" id="txtAvailableCash2" class="txtBox form-control" style="font-size: 20px; font-weight: bold;" readonly="readonly" />
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="row" style="display: none;" id="dvAuthorityUser2">
                <label>
                    Authority Person
                </label>
                <select id="ddlDiscountUser3" class="form-control" onchange="loadPassword(this);"></select>
                <input type="hidden" value="0" id="hfManagerPassword2" />
            </div>
            <div class="row" style="border-top: 1px solid #dadada; border-bottom: 1px solid #dadada; padding: 5px 0 5px 0; margin-top: 10px; display: none;" runat="server" id="dvServiceCharges2">
                <div class="col-md-3" style="padding-top: 10px;">
                    <label runat="server" id="lblServiceChg2">
                        Service Charges
                    </label>
                </div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #7dab49;"
                        onclick="DiscTypeService(this);" id="percentageService"
                        value="0">
                        %age</button>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #919399;"
                        onclick="DiscTypeService(this)" id="valueService"
                        value="1">
                        Value</button>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-3">
                    <input type="text" id="txtService2" runat="server" class="form-control" style="font-size: 20px; font-weight: bold;"
                        onkeyup="CalculateServiceChages();" onkeypress="return onlyDotsAndNumbers(this,event);" />
                </div>
            </div>
            <div class="row" style="border-top: 1px solid #dadada; border-bottom: 1px solid #dadada; padding: 5px 0 5px 0; margin-top: 10px; display: none;" runat="server" id="dvCustomerGST">
                <div class="col-md-3" style="padding-top: 10px;">
                    <label runat="server" id="Label1">
                        GST
                    </label>
                </div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #7dab49;"
                        id="percentageCustomerGST"
                        value="0">
                        %age</button>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <button type="button" class="btn btn-toolbar" style="min-width: 120px; font-size: 15px; color: #FFF; background-color: #919399; display: none;"
                        id="valueServiceCustomerGST"
                        value="1">
                        Value</button>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-3">
                    <input type="text" id="txtCustomerGSTValue" runat="server" class="form-control" style="font-size: 20px; font-weight: bold;"
                        onkeyup="ApplyCustomerGST();" onkeypress="return onlyDotsAndNumbers(this,event);" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            </div>
            <div class="col-md-6">
                <div class="col-md-5 btn-mar">
                    <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: -143px; font-size: 15px; color: #FFF; background-color: #3c8d75;"
                        id="btnSave2" onclick="UpdateOrder();">
                        Save</button>
                </div>
                <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                    <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: -133px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                        id="btnCancel2" onclick="ClearOnCancel2();">
                        Cancel</button>
                </div>
                <div class="col-md-5" style="margin-top: 15px;">
                </div>
            </div>
    </div>
    <div class="clear">
    </div>
</div>
<div style="display: none;">
    <table class="table table-striped table-bordered table-hover table-condensed cf">
        <tbody id="tble-discount-limit2"></tbody>
        <tbody id="tble-discount-user2"></tbody>
    </table>
</div>
<div id="DiscountValidation" style="max-width: 400px; display: none; z-index: 5000; top: 20%; left: 35%; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px">
    <div class="row">
        <div class="col-md-12">
            <div class="text">
                Discount Authentication
            </div>
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label id="lblDiscountAuthType">
                        Discount Type
                    </label>
                    <select name="ddlDiscountAuthType" id="ddlDiscountAuthType" class="form-control">
                        <option value="1">Value</option>
                        <option value="2">%</option>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Discout
                    </label>
                    <input type="text" id="txtDiscountAuth" autocorrect="off" runat="server" class=" form-control" autocomplete="off" value="" onkeypress="return onlyDotsAndNumbers(this,event);" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        Remarks
                    </label>
                    <input type="text" id="txtDiscountAuthRemarks" autocorrect="off" runat="server" class=" form-control" autocomplete="off" value="" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12" style="margin-top: 5px; margin-left: 60px;">
                <div class="col-md-8">
                    <label>
                        User ID
                    </label>
                    <input type="text" id="txtDiscountAuthUserID" autocorrect="off" runat="server" class=" form-control" autocomplete="off" value="" />
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

                    <input type="password" id="txtDiscountAuthUserPass" runat="server" class=" form-control" autocomplete="off" value="" />
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
                            id="btnSaveAuthicateDiscount" onclick="AuthenticateDiscountUser();">
                            Save</button>
                    </div>
                    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
                        <button class="btn btn-toolbar" type="button" style="min-width: 100px; margin-left: 45px; font-size: 15px; color: #FFF; background-color: #f94d72;"
                            id="btnCancelAuthenticateDiscount" onclick="CancelAuthenticateDiscount();">
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
