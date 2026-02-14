<style>
    @media only screen and (max-width: 1440px) {
        .keypadpop {
            top: 20% !important;
            left: 30% !important;
        }
    }
    @media only screen and (max-width: 1248px) {
  .keypadpop{
      top: 20% !important; 
        left: 30% !important;
  }
}
</style>
<div class="keypadpop" id="Customkeyboad" style="max-width: 500px; width: 99%; display: none; z-index: 5000; top: 15%; left: 30%;
 position: absolute; background-color: #c6c6c6; padding: 10px; border: 1px solid #000; min-height: 450px; border-radius: 7px;">
<input id="hfKeyBoadTextBoxID" runat="server" type="hidden"/>
<div id="numericInput3">
    <input type="text" id="txtKeyBoad"  runat="server" onkeypress="return onlyNumbers(this,event);"
         style="text-align:right;height:60px;border-style:solid;border-color:lightblue;border-width:medium;font-size:25px;font-weight:bold;width:100%;    padding: 6px 12px;"/>
    <table id="keypad3">
        <tr>
            <td class="key3">1</td>
            <td class="key3">2</td>
            <td class="key3">3</td>
        </tr>
        <tr>
            <td class="key3">4</td>
            <td class="key3">5</td>
            <td class="key3">6</td>
        </tr>
        <tr>
            <td class="key3">7</td>
            <td class="key3">8</td>
            <td class="key3">9</td>
        </tr>
        <tr>
            <td class="Custombtn3">CLEAR</td>
            <td class="key3">0</td>
            <td class="Custombtn23">DONE</td>
        </tr>
    </table>
</div>
</div>