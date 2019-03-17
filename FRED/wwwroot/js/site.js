

/*****Mark's js variables*****/
let idNum = 0;
let idVar = 0;
let idArray = new Array();
let errorMsg = null;
let enrollTexts;
let count = 0;
let profileArray = [];
let profileEntry;
/***********************/

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


let KeyCtrl;

function KeyboardCtrl(status) {    
    KeyCtrl = status;
}

let particles = new Array();
let car = document.getElementById('carContainer');

for (let i = 0; i < 25; i++) {
    let exhaust = new Exhaust(470, 500);
    let exhaustPart = document.createElement('div');
    exhaustPart.setAttribute('class', 'exhaustSmoke');
    exhaustPart.setAttribute('style', 'width: ' + exhaust.size + 'px;' +
        'height: ' + exhaust.size + 'px;');
    if (document.title == "Home - FRED") {
        car.appendChild(exhaustPart);
    }
    
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

//let faceIsOpen = false;

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
            top: 200
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

function FredSees() {
    $('#fredSeesForm').submit();
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

function CloseVoicePanel() {    
    $('#closeVoicePanelForm').submit();
}

// refresh page and and run method
window.onload = function () {
    var reloading = sessionStorage.getItem("reloading");
    if (reloading) {
        sessionStorage.removeItem("reloading");
        //ToggleFacePanel();
    }
    
    setTimeout(function () {
        $('#screen').fadeOut(2000)  
        $('#displayText').fadeOut(2000)
    }, 3000);


    /*******************************Mark's***********************************/
    if (document.title == "UserControls - FRED")
    {
        if ($("input[name=profileName]").val() != "") {
            $("input[name=profileName]").prop("disabled", true);
        }

        if ($("input[name=profileName]").prop("disabled") && document.getElementById("enrollBtn1").disabled && document.getElementById("enrollBtn2").disabled) {
            document.getElementById("enrollBtn2").disabled = false;
        }
        else {
            document.getElementById("enrollBtn2").disabled = true;
        }

        if ($("input[name=profileName]").prop("disabled") && document.getElementById("enrollBtn1").disabled && document.getElementById("enrollBtn2").disabled) {
            document.getElementById("confEnroll").disabled = false;
        }

        if (document.getElementById("enrolledVoices").style.display == "block") {
            document.getElementById("enroll").style.pointerEvents = "none";
        }
    }
    /************************************************************************/
}

function Refresh() {
    sessionStorage.setItem("reloading", "true");
    document.location.reload();
}


/*
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
if (document.title == "UserControls - FRED") {
    let slider = document.getElementById("mySpeed");
    let output = document.getElementById("speed");
    output.innerHTML = slider.value; // Display the default slider value

    slider.oninput = function () {
        output.innerHTML = this.value;
    }

    slider.onchange = function () {
        this.form.submit();
    }


    /*Mark's js*/
    function ShowWindow(value) {
        event.preventDefault(); // stop submission of form
        if (value == "enroll") {
            document.getElementById(value).style.display = "block";
            document.getElementById("updKB").style.display = "none";
            document.getElementById("enroll").style.pointerEvents = "initial";
        }
        else if (value == "updKB") {
            document.getElementById(value).style.display = "block";
            document.getElementById("enroll").style.display = "none";
            document.getElementById("enrolledVoices").style.display = "none";
        }
        if (idNum == 0) // add question and answer field if none is in there
        {
            AddQnA();
        }
    }

    function AddQnA() {
        idNum++;

        if (idNum <= 10) {
            idVar = idNum;
            while (idArray.includes("" + idVar)) {
                idVar++;
            }

            idArray.push("" + idVar);
            let quest = document.createElement("input");
            let answer = document.createElement("input");
            let remove = document.createElement("i");

            remove.id = idVar;
            quest.id = "question" + idVar;
            answer.id = "answer" + idVar;

            remove.className = "fas fa-times-circle fa-lg deleteQnA";

            remove.style = "float: right; margin-top: 22px; cursor: pointer";
            quest.style = "margin-top: 15px; margin-left: 10px; margin-right: 2px";
            answer.style = "margin-top: 15px; margin-left: 2px";

            quest.placeholder = "enter question";
            answer.placeholder = "enter answer";

            let submitQnA = document.getElementById("submitQnA");
            let addQnA = document.getElementById("addQnA");
            let brIn = document.getElementsByClassName("brIn");
            let br = document.createElement("br");
            br.className = "brIn";

            document.getElementById("QnAForm").removeChild(submitQnA);
            document.getElementById("QnAForm").removeChild(addQnA);
            document.getElementById("QnAForm").removeChild(brIn[0]);

            document.getElementById("QnAForm").appendChild(quest);
            document.getElementById("QnAForm").appendChild(answer);
            document.getElementById("QnAForm").appendChild(remove);

            document.getElementById("QnAForm").appendChild(br)
            document.getElementById("QnAForm").appendChild(br)

            document.getElementById("QnAForm").appendChild(submitQnA);
            document.getElementById("QnAForm").appendChild(addQnA);

            document.getElementById(remove.id).setAttribute("onclick", "DeleteQnA('" + idVar + "');");
        }
        else {
            idNum--;
        }
    }

    function DeleteQnA(elem) {
        if (idNum > 1) {
            document.getElementById("QnAForm").removeChild(document.getElementById("question" + elem));
            document.getElementById("QnAForm").removeChild(document.getElementById("answer" + elem));
            document.getElementById("QnAForm").removeChild(document.getElementById(elem));
            idArray.splice(idArray.indexOf(elem), 1);
            idNum--;
        }
    }

    function CloseWindow(id) {
        document.getElementById(id).style.display = "none";
        if (id == "enroll") {
            if (document.getElementById("enrollBtn1").innerHTML != "Voice 1") // only post if there's actually been a recording
            {
                $("#cancE").click();
            }
            else {
                document.getElementById("enrolledVoices").style.display = "none";
                document.getElementById("name").value = "";
                document.getElementById("desc").value = "";
                document.getElementById("desc").disabled = true;
                document.getElementById("enrollBtn1").disabled = true;
            }
        }

        if (id == "enrolledVoices") {
            document.getElementById("enroll").style.pointerEvents = "initial";
        }
    }


    function Record()// ensures the audio gif displays as recording starts
    {
        document.getElementById("recordAud").style.display = "block";
    }

    function StopRecording() {
        document.getElementById("recordAud").style.display = "none";
    }

    function RecVoice(BtnId) {
        if (BtnId == "enrollBtn1") {
            enrollTexts += ":" + $("input[name=profileDesc]").val();
            $("#subText").val(enrollTexts); //assign val in in enrollTexts to hidden text input
        }
        document.getElementById("recordAud").style.display = "block";
        $('#recVoice').submit();
    }

    $("input[name=profileName]").focusin(function () {
        $("input[name=profileName]").change(function () {
            if ($("input[name=profileName]").val() != "") {
                enrollTexts = $("input[name=profileName]").val(); // stores val in profile name away
                document.getElementById("desc").disabled = false;
            }
            else {
                document.getElementById("desc").disabled = true;
            }
        });
    });

    $("input[name=profileDesc").focusin(function () {
        $("input[name=profileDesc]").change(function () {
            if ($("input[name=profileDesc]").val() != "") {
                document.getElementById("enrollBtn1").disabled = false;
            }
            else {
                document.getElementById("enrollBtn1").disabled = true;
            }
        });
    });

    function ShowEnrollments() {
        document.getElementById("enrolledVoices").style.display = "table";
        document.getElementById("enroll").style.pointerEvents = "none";
    }

    function UpdateKB() {
        let qNa = "";
        errorMsg = null;
        for (i = 0; i < idNum; i++) {
            qValue = document.getElementById("question" + idArray[i]).value;
            aValue = document.getElementById("answer" + idArray[i]).value;

            if ((qValue != "" && aValue == "") || (qValue == "" && aValue != "") || (qValue == "" && aValue == "")) {
                document.getElementById("errMsg").innerHTML = "";
                if (errorMsg != null)
                    document.getElementById("errMsg").removeChild(errorMsg);

                document.getElementById("errMsg").style.display = "block";

                errorMsg = document.createElement("p");
                document.getElementById("errMsg").style.height = "65px";
                errorMsg.innerHTML = "Please enter question and answer pair!";

                document.getElementById("errMsg").appendChild(errorMsg);

                setTimeout(function () {
                    $("#errMsg").fadeOut(2000)
                }, 3000);

                return;
            }
            document.getElementById("question" + idArray[i]).disabled = true;
            document.getElementById("answer" + idArray[i]).disabled = true;
            qNa += qValue + ":" + aValue;

            if (i < (idNum - 1)) {
                qNa += ";";
            }
        }

        document.getElementById("QnA").value = qNa;
        $('#QnAForm').submit();
        document.getElementById("addQnA").disabled = true;
        idNum = 0; // reset q and a input fields
        idArray = [];
    }

    function AssignProfileId(profileId) // assigns correct profile id to hidden text field value
    {
        if (profileId == "") {
            let updateInfo = "";
            for (let i = 0; i < profileArray.length; i++) {
                updateInfo += profileArray[i] + ":";
                updateInfo += document.getElementById("profileId_" + profileArray[i]).value + ":";
                updateInfo += document.getElementById("nameInp_" + profileArray[i]).value + ":";
                if (i == profileArray.length - 1) {
                    updateInfo += document.getElementById("descInp_" + profileArray[i]).value;
                }
                else {
                    updateInfo += document.getElementById("descInp_" + profileArray[i]).value + ";";
                }
            }
            $("#profileId").val(updateInfo);
        }
        else {
            $("#profileId").val(profileId);
        }
    }

    if (document.getElementById("errMsg").style.display == "block") {
        let errorMsg = document.createElement("p");

        errorMsg.innerHTML = "Cannot connect to server!";

        document.getElementById("errMsg").appendChild(errorMsg);
        setTimeout(function () {
            $("#errMsg").fadeOut(2000)
        }, 3000);
    }

    // enabling edit button on enrolled voice name and description
    function Edit(value)
    {
        count = value * 2;
        if (profileArray.includes(value))
        {
            let indexPos = profileArray.indexOf(value);
            profileArray.splice(indexPos, 1);
            document.getElementById("edit_" + value).innerHTML = "Edit";
            document.getElementById("nameInp_" + value).setAttribute("hidden", "");
            document.getElementById("nameInp_" + value).removeAttribute("required");
            document.getElementById("descInp_" + value).setAttribute("hidden", "");
            document.getElementById("descInp_" + value).removeAttribute("required");
            profileEntry[count].removeAttribute("hidden");
            profileEntry[count + 1].removeAttribute("hidden");
        }
        else
        {
            document.getElementById("nameInp_" + value).removeAttribute("hidden");
            document.getElementById("nameInp_" + value).setAttribute("required", "");
            document.getElementById("descInp_" + value).removeAttribute("hidden");
            document.getElementById("descInp_" + value).setAttribute("required", "");
            profileEntry = document.getElementsByClassName("nameDesc");
            profileEntry[count].setAttribute("hidden", "");
            profileEntry[count + 1].setAttribute("hidden", "");
            document.getElementById("edit_" + value).innerHTML = "Cancel";
            count = 0;
            profileArray.push(value);
        }

        if (profileArray.length > 0)
        {
            document.getElementById("submitProfileEdit").removeAttribute("hidden");
        }
        else
        {
            document.getElementById("submitProfileEdit").setAttribute("hidden", "");
        }
    }
}