
$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            if (fileName.length > 20) {
                fileName = fileName.substring(0, 20) + "...";
            }
            $(this).next('.custom-file-label').html(fileName);
        }
    );
});

var loadFile = function (event) {
    var output = document.getElementById('output');
    //$("#output").addClass("report_image");
    output.src = URL.createObjectURL(event.target.files[0]);
};