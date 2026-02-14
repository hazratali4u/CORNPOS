<style>
    @media only screen and (max-width: 1440px) {
        .keypadpop {
            top: 20% !important;
            left: 30% !important;
        }
    }

    @media only screen and (max-width: 1248px) {
        .keypadpop {
            top: 20% !important;
            left: 30% !important;
        }
    }
</style>
<div class="popUpProvissionalBill" id="popUpBill" style="max-width: 500px; width: 99%; display: none; z-index: 5000; top: 15%; left: 30%; position: absolute; background-color: #c6c6c6; padding: 10px; border: 1px solid #000; height: 450px; border-radius: 7px; overflow-y:  auto;">
    <div class="row">
        <div class="col-md-5">
            <label id="lblHead" style="font: bold 17px sans-serif;">Provissional Bill</label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            <label id="lblServiceType"></label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            Date:
            <label id="BillDate"></label>
        </div>
        <div class="col-md-5">
            O-T:
            <label id="BillOT"></label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5">
            Order No:
            <label id="lblOrderNo"></label>
        </div>
        <div class="col-md-5">
            Bill No:
            <label id="lblBillNo"></label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered table-hover">
                <thead>
                    <tr>
                        <th>Item Name</th>
                        <th>Quantity</th>
                        <th>Rate</th>
                        <th>Amount</th>
                    </tr>
                </thead>
                <tbody id="dvBillDetail"></tbody>
            </table>
        </div>
        <div class="row" style="text-align: right; padding-right: 32px;">
            <div class="col-md-12">
                <label>Total:</label>
            <label id="lblTotal"></label>
            </div>
        </div>
        <div class="row" style="text-align: right; padding-right: 32px;">
            <div class="col-md-12">
                <label id="lblDiscountTextBill"></label>
                <label id="lblDiscountBill"></label>
            </div>
        </div>
        <div class="row" style="text-align: right; padding-right: 32px;">
            <div class="col-md-12">
                <label id="GSTtextBill"></label>
                <label id="GSTValueBill"></label>
            </div>
        </div>
        <div class="row" style="text-align: right; padding-right: 32px;">
            <div class="col-md-12">
                <label id="SCtextBill"></label>
                <label id="SCValueBill">Ser. Charges: </label>
            </div>
        </div>
        <div class="row" style="text-align: right; padding-right: 32px;">
            <div class="col-md-12">
                <label>Grand Total:</label>
            <label id="lblGrandTotal"></label>
            </div>
        </div>
    </div>
    <div class="col-md-5 btn-mar" style="margin-left: 10px;">
        <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #f94d72;" id="btnClose">Close</button>
    </div>
</div>
