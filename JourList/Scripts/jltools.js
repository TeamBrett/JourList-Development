
function nodollar(amt) { return Math.round((amt + "").replace(/\$/g, '')); };
function todollar(amt) { return "$" + ((nodollar(amt) * 100) / 100); };
function isnavpress(keycode) { if (keycode >= 8 && keycode <= 46) { return true; }; };
function isdecimal(keycode) { if (event.which != 190 && event.which != 110) { return true; }; };
function isnumpress(keycode) {
    //          numbers                                period                            number pad                        decimal
    if ((event.which <= 57 && event.which >= 48) || event.which == 190 || (event.which <= 105 && event.which >= 96) || event.which == 110
    //      delete       backspace
         || keycode == 46 || keycode == 8) {
        return true;
    };
};


function mapKeyPressToActualCharacter(isShiftKey, characterCode) {
    if (characterCode === 27 || characterCode === 8 || characterCode === 9 || characterCode === 20 || characterCode === 16 || characterCode === 17 || characterCode === 91 || characterCode === 13 || characterCode === 92 || characterCode === 18) {
        return false;
    }
    if (typeof isShiftKey != "boolean" || typeof characterCode != "number") {
        return false;
    }
    var characterMap = [];
    characterMap[192] = "~";
    characterMap[49] = "!";
    characterMap[50] = '@@';
    characterMap[51] = "#";
    characterMap[52] = "$";
    characterMap[53] = "%";
    characterMap[54] = "^";
    characterMap[55] = "&";
    characterMap[56] = "*";
    characterMap[57] = "(";
    characterMap[48] = ")";
    characterMap[109] = "_";
    characterMap[107] = "+";
    characterMap[219] = "{";
    characterMap[221] = "}";
    characterMap[220] = "|";
    characterMap[59] = ":";
    characterMap[222] = "\"";
    characterMap[188] = "<";
    characterMap[190] = ">";
    characterMap[191] = "?";
    characterMap[32] = " ";
    var character = "";
    if (isShiftKey) {
        if (characterCode >= 65 && characterCode <= 90) {
            character = String.fromCharCode(characterCode);
        } else {
            character = characterMap[characterCode];
        }
    } else {
        if (characterCode >= 65 && characterCode <= 90) {
            character = String.fromCharCode(characterCode).toLowerCase();
        } else {
            character = String.fromCharCode(characterCode);
        }
    }
    return character;
};
