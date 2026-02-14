
<div class="modifierpop" id="divModifier" style="width: 60%; display: none; z-index: 5000; top: 15%; left: 20%; position: absolute; background-color: #c6c6c6; 
padding: 10px; border: 1px solid #000; min-height: 50vh; max-height: 60vh">
    <div class="row" style="vertical-align: middle">
        <div class="col-md-3">
            <div id="modifierMessage" style="color:red;font-size: small;font-weight:bold; margin-top: 2%"></div>
        </div>
        <div class="col-md-7" style="text-align:right;">
            <div id="divModifierParentName" style="color:red;font-size:small;font-weight:bold; margin-top: 2%"></div>
        </div>
        <div class="col-md-2" style="text-align:right;">
            <button class="btn btn-toolbar" type="button"  style="background-color: #00a65a;border-color: #008d4c;color: #fff;"
                id="btnModifierDone" onclick="ModifierDone();">
                Done
            </button>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 item_pd_left">
            <div class="bg-product scrolla col" style="height:50vh;margin-left:10px !important;">
            <div class="pad" id="dv_lstModifyCategory" style="display: none;">
            </div>
            </div>
        </div>
        <div class="col-md-9">
            <div class="bg-product scrolla col" style="height:50vh;">
                <div class="pad" id="dv_lstModifyProducts" style="display: none;">
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
</div>