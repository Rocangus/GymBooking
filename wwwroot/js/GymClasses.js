var form = document.getElementById("classIndex-form");

var box = document.getElementById("historic")

document.addEventListener("DOMContentLoaded", function() {
    var state = JSON.parse(localStorage.getItem("selected"));
    if (state) {
        box.checked = true;
    }
    localStorage.removeItem("selected");
});

document.getElementById("historic").addEventListener("click", function()
{
    localStorage.setItem("selected", JSON.stringify(box.checked));
    form.submit();
});