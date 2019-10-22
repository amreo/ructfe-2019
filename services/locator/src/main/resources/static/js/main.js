function setLoginHandlers() {
    const form = $("#sbm-frm");
    const usernameField = $("#usnm");
    const passwordField = $("#pswd");

    form.on("submit", function (ev) {
        const username = usernameField.val();
        const password = passwordField.val();
        const data = JSON.stringify({
            "name": username,
            "password": password
        });
        $.post("/login", data)
            .fail(function (data) {
                alert("Wrong password");
            })
            .done(function (data) {
                document.location.href = "/";
            })
        ;
        return false;
    });
}

function setRegisterHandlers() {
    const form = $("#sbm-frm");
    const usernameField = $("#usnm");
    const passwordField = $("#pswd");
    const powerField = $("#power");
    const colorField = $("#color");

    form.on("submit", function (ev) {
        const username = usernameField.val();
        const password = passwordField.val();
        const power = powerField.val();
        const color = colorField.val().substr(1, 6);
        const data = JSON.stringify({
            "name": username,
            "password": password,
            "power": parseInt(power),
            "color": color
        });
        $.post("/register", data)
            .fail(function (data) {
                alert(data);
            })
            .done(function (data) {
                document.location.href = "/";
            })
        ;
        return false;
    });
}

function draw() {
    const canvas = document.getElementById("myCanvas");
    const width = document.body.clientWidth;
    const height = document.body.clientHeight;
    canvas.width = width;
    canvas.height = height;
    const ctx = canvas.getContext("2d");
    ctx.fillStyle = "#000000";
    ctx.fillRect(0, 0, width, height);
}
