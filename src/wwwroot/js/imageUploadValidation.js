$(function () {
    console.log("Script loaded!");

    // DOM Elements
    let saveBtn = $("#save"); // Save (submit) button
    let imageInput = $("#image-input"); // Image URL Input Text Box
    let urlErrorText = $("#image-error-text"); // Image URL error text
    let uploadErrorText = $("#image-upload-error-text"); // Image upload error text
    let imageUpload = $("image-upload");

    let validImageUrl = imageInput.val().length >= 0;
    let validImageUpload = $("#image-upload")[0].files.length > 0;

    // Disable Save button
    saveBtn.attr("disabled", "disabled");

    // Validates if the user has provided us with either a valid Image URL or File Upload
    function validateImageInput() {
        if ((validImageUrl && imageInput.val() !== "") || validImageUpload) {
            // Remove error messages
            imageInput.removeClass("input-error");
            urlErrorText.removeClass("visually-hidden").addClass("visually-hidden");
            uploadErrorText.removeClass("visually-hidden").addClass("visually-hidden");

            // Enable Save button
            saveBtn.removeAttr("disabled");
            saveBtn.ariaInvalid = false;

        } else {
            saveBtn.attr("disabled", "disabled");
            saveBtn.ariaInvalid = true;
        }
    }

    // Image Url validation
    imageInput.blur(function () {
        console.log("Image Url input detected");
        let imageUrls = $(this).val().split(",");


        // Check each url
        imageUrls.forEach((url) => {
            new Promise((resolve) => {
                const img = new Image();
                img.src = url;

                // URL Contains image - Valid URL
                img.onload = () => {
                    resolve(true);
                    validImageUrl = true;
                };

                // URL does not contain image - invalid URL
                img.onerror = () => {
                    resolve(false);
                    validImageUrl = false;
                };
            });
        });

        if ($(this).val() === "") {
            validImageUrl = true;
            console.log("Empty url value: ", validImageUrl);
        }

        if (validImageUrl) {
            imageInput.removeClass("input-error");
            urlErrorText.removeClass("visually-hidden").addClass("visually-hidden");
        } else {
            // Invalid image url message
            console.log("Error classes should be added now");
            imageInput.removeClass("input-error").addClass("input-error");
            urlErrorText.removeClass("visually-hidden");
        }

        validateImageInput();
    });

    // Image upload validation
    $(document).on("input", "input:file", function (e) {

        let uploadFileCount = e.target.files.length;
        validImageUpload = uploadFileCount > 0;
        console.log("Valid image upload? ", validImageUpload);
        console.log("Checking Image Url: ", validImageUrl);
        // Check if a url is provided
        if (validImageUpload) {
            imageUpload.removeClass("input-error");
            uploadErrorText.removeClass("visually-hidden").addClass("visually-hidden");
        } else {
            // Invalid image url message
            console.log("Error classes should be added now");
            imageUpload.removeClass("input-error").addClass("input-error");
            uploadErrorText.removeClass("visually-hidden");
        }

        validateImageInput();
    });
});