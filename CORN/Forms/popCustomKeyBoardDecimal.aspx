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
<div class="keypadpop" id="CustomkeyboadDecimal" style="max-width: 500px; width: 99%; display: none; z-index: 5000; top: 15%; left: 30%; position: absolute; background-color: #c6c6c6; padding: 10px; border: 1px solid #000; min-height: 480px; border-radius: 7px;">
    <input id="hfKeyBoadTextBoxIDDecimal" runat="server" type="hidden" />
    <div id="numericInput3Decimal">
        <table id="keypad3Decimal">
            <tr>
                <td colspan="3">
                    <table style="width: 100%;">
                    <tr>
                        <td style="width: 35%;">
                            <span class="Custombtn3Decimal" style="width: 88%;">CLEAR</span>
                        </td>
                        <td style="width: 65%;">
                            <input type="text" id="txtKeyBoadDecimal" runat="server" onkeypress="return onlyDotsAndNumbers(this,event);"
                                style="text-align: right; height: 88px; border-style: solid; border-color: lightblue; border-width: medium; font-size: 25px; font-weight: bold; width: 100%; padding: 6px 12px;" />
                        </td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td class="key3Decimal">1</td>
                <td class="key3Decimal">2</td>
                <td class="key3Decimal">3</td>
            </tr>
            <tr>
                <td class="key3Decimal">4</td>
                <td class="key3Decimal">5</td>
                <td class="key3Decimal">6</td>
            </tr>
            <tr>
                <td class="key3Decimal">7</td>
                <td class="key3Decimal">8</td>
                <td class="key3Decimal">9</td>
            </tr>
            <tr>                
                <td class="key3Decimal">.</td>
                <td class="key3Decimal">0</td>
                <td class="Custombtn23Decimal">DONE</td>
            </tr>
        </table>
    </div>
</div>
