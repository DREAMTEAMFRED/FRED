

let faceIsOpen = false;

function ToggleFacePanel() {
    if (!faceIsOpen) { // open panel        
        $('#addFacePanel').animate({
            top: 150
        }, 220, 'swing');
        faceIsOpen = true;
    }
    else { // close panel        
        $('#addFacePanel').animate({
            top: -800
        }, 220, 'swing');
        faceIsOpen = false;
    }
} // TogglePanel()

let addPersonIsOpen = false;

function ToggleAddPersonPanel() {
    if (!addPersonIsOpen) { // open panel        
        $('#addPersonPanel').animate({
            top: 150
        }, 220, 'swing');
        addPersonIsOpen = true;
    }
    else { // close panel        
        $('#addPersonPanel').animate({
            top: -800
        }, 220, 'swing');
        addPersonIsOpen = false;
    }
} // TogglePanel()



/*
let keys = {
    37: 'left',
    39: 'right',
    40: 'down',
    38: 'up',
    32: 'space',
    65: 'A',
    66: 'B',
    67: 'C',
    68: 'D',
    69: 'E',
    70: 'F',
    71: 'G',
    72: 'H',
    73: 'I',
    74: 'J',
    75: 'K',
    76: 'L',
    77: 'M',
    78: 'N',
    79: 'O',
    80: 'P',
    81: 'Q',
    82: 'R',
    83: 'S',
    84: 'T',
    85: 'U',
    86: 'V',
    87: 'W',
    88: 'X',
    89: 'Y',
    90: 'Z',
};

let keyDown = false;

document.body.onkeydown = function (e) {

    if (typeof keys[e.keyCode] !== 'undefined') {
        let forward = document.getElementById("forwardBtn");
        let backward = document.getElementById("backwardBtn");
        let left = document.getElementById("leftBtn");
        let right = document.getElementById("rightBtn");
        let camUp = document.getElementById("camUp");
        let camDown = document.getElementById("camDown");
        let camRight = document.getElementById("camRight");
        let camLeft = document.getElementById("camLeft");
        let camHome = document.getElementById("camHome");
        let stop = document.getElementById("stopBtn");
        let home = document.getElementById("homeBtn");

        switch (e.keyCode) {
            case 38: {
                forward.click();
                break;
            }
            case 40: {
                backward.click();
                break;
            }
            case 37: {
                left.click();
                break;
            }
            case 39: {
                right.click();
                break;
            }
            case 87: {
                camUp.click();
                break;
            }
            case 83: {
                camDown.click();
                break;
            }
            case 65: {
                camLeft.click();
                break;
            }
            case 68: {
                camRight.click();
                break;
            }
            case 66: {
                camHome.click();
                break;
            }
            case 32: {
                stop.click();
                home.click();
                break;
            }
            default: {
                KeyHandler(keys[e.keyCode]);
                break;
            }
        }
    }
};

document.body.onkeyup = function (e) {

    if (typeof keys[e.keyCode] !== 'undefined') {
        let stop = document.getElementById("stopBtn");
        let home = document.getElementById("homeBtn");

        switch (e.keyCode) {
            case 38: {
                stop.click();
                break;
            }
            case 40: {
                stop.click();
                break;
            }
            case 37: {
                home.click();
                break;
            }
            case 39: {
                home.click();
                break;
            }
            default: {
                KeyHandler(keys[e.keyCode]);
                break;
            }
        }
    }
};
*/

let slider = document.getElementById("mySpeed");
let output = document.getElementById("speed");
output.innerHTML = slider.value; // Display the default slider value

slider.oninput = function () {
    output.innerHTML = this.value;
}

slider.onchange = function () {
    this.form.submit();
}
