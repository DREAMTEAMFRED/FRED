function FadeOut() {       
    setTimeout(function () {
        $('#screen').fadeOut(2000)
        $('#displayText').fadeOut(2000)
    }, 3000);
}

let KeyCtrl;

function KeyboardCtrl(status) {    
    KeyCtrl = status;
}

let slider = document.getElementById("mySpeed");
let output = document.getElementById("speed");
output.innerHTML = slider.value; // Display the default slider value

slider.oninput = function () {    
    output.innerHTML = this.value;
}

slider.onchange = function () {    
    this.form.submit();
}

let keyDown = false;
    
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
    
document.body.onkeydown = function (e) {

    if (typeof keys[e.keyCode] !== 'undefined' && KeyCtrl == 'grid') {
        let forward = document.getElementById("forwardBtn");
        let backward = document.getElementById("backwardBtn");
        let left = document.getElementById("leftBtn");
        let right = document.getElementById("rightBtn");
        let camUp = document.getElementById("camUpBtn");
        let camDown = document.getElementById("camDownBtn");
        let camRight = document.getElementById("camRightBtn");
        let camLeft = document.getElementById("camLeftBtn");
        let camHome = document.getElementById("camHomeBtn");
        let stop = document.getElementById("stopBtn");
        let home = document.getElementById("homeBtn");

        switch (e.keyCode) {
            case 38: {                                            
                forward.submit();                                    
                break;
            }
            case 40: {                
                backward.submit();
                break;
            }
            case 37: {
                left.submit();
                break;
            }
            case 39: {
                right.submit();
                break;
            }
            case 87: {
                camUp.submit();
                break;
            }
            case 83: {
                camDown.submit();
                break;
            }
            case 65: {
                camLeft.submit();
                break;
            }
            case 68: {
                camRight.submit();
                break;
            }
            case 66: {
                camHome.submit();
                break;
            }
            case 32: {
                stop.submit();
                home.submit();
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
        if (typeof keys[e.keyCode] !== 'undefined' && KeyCtrl == 'grid') {
            let stop = document.getElementById("stopBtn");
            let home = document.getElementById("homeBtn");

            switch (e.keyCode) {
                case 38: {
                    stop.submit();
                    break;
                }
                case 40: {
                    stop.submit();
                    break;
                }
                case 37: {
                    home.submit();
                    break;
                }
                case 39: {
                    home.submit();
                    break;
                }
                default: {
                    KeyHandler(keys[e.keyCode]);
                    break;
                }
            }
        }
    };



////// * Car bouncing * \\\\\\

setInterval('BouncingCar()', 150);
let bounce = document.getElementById('robotCar');
let dir = 'up';
let topPos = 150;

function BouncingCar() {
    if (dir == 'up') {
        $(bounce).animate(
            {
                top: topPos
            }, 50, 'swing')
        dir = 'down'
    }
    else {
        $(bounce).animate(
            {
                top: topPos - 2
            }, 50, 'swing')
        dir = 'up'
    }
}

//////  * Create car exhaust *  \\\\\\

let particles = new Array();
let car = document.getElementById('carContainer');

for (let i = 0; i < 25; i++) {
    let exhaust = new Exhaust(470, 500);
    let exhaustPart = document.createElement('div');
    exhaustPart.setAttribute('class', 'exhaustSmoke');
    exhaustPart.setAttribute('style', 'width: ' + exhaust.size + 'px;' +
        'height: ' + exhaust.size + 'px;');
    car.appendChild(exhaustPart);
    particles.push(exhaust);    
}

let exhaustTimer = setInterval('BlowSmoke()', 25);
let allSmoke = document.getElementsByClassName('exhaustSmoke');

function BlowSmoke() {
    for (let i = 0; i < particles.length; i++) {
        particles[i].Move();
        let y = particles[i].y;
        let x = particles[i].x;
        $(allSmoke[i]).animate(
            {
                top: y,
                left: x
            }, 10, 'swing');
    }
} 



function OpenAddPersonPanel() {
    $('#addPersonForm').submit();
}

function CloseAddPersonPanel() {
    $('#closePersonForm').submit();
}

function DetectFace() {
    $('#detectFaceForm').submit();
}

function FredReads() {
    $('#fredReadsForm').submit();
}

function SubmitForm() {
    $('#addFaceFormSubmit').submit();
    ToggleTakePicPanel();
}

function CloseWindow() {
    $('#closeWindow').submit();
}

function CloseFacePanel() {
    $('#closeFacePanelForm').submit();
}

function CloseControlPanel() {        
    $('#closeCtrlPanel').submit();    
}

function CloseVisionPanel() {    
    $('#closeVisionPanelForm').submit();
}









