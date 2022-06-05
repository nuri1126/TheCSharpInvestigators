$(function () {
    console.log("Script loaded!");

    // DOM Elements
    let saveBtn = $("#save"); // Save (submit) button
    let imageInput = $("#image-input"); // Image URL Input Text Box
    let urlErrorText = $("#image-error-text"); // Image URL error text
    let uploadErrorText = $("#image-upload-error-text"); // Image upload error text
    let imageUpload = $("#image-upload"); // File upload element

    let validImageUrl = imageInput.val().length >= 0;
    let validImageUpload = imageUpload[0].files.length > 0;

    // Set initial Save button attribute
    if(validateImageUrlAndUpload()) {
        saveBtn.removeAttr("disabled");
    } else {
        // Disable Save button
        saveBtn.attr("disabled", "disabled");    
    }

    // Validates if the user has provided us with either a valid Image URL or File Upload
    function validateImageUrlAndUpload() {
        /*
        Three conditions need to be tests for verifying either Image URL(s) and/or Image upload:
        1. A valid image url is provided but the no images are to be uploaded
        2. No image url provided but images are to be uploaded
        3. Valid image urls and images are to be uploaded
         */
        
        if(  (validImageUrl && imageInput.val().length > 0 && imageUpload[0].files.length === 0 ) || 
             (validImageUrl && imageInput.val().length === 0 && validImageUpload) ||
             (validImageUrl && imageInput.val().length > 0 && validImageUpload)
        ) {
        // if ((validImageUrl && imageInput.val() !== "") || validImageUpload) {
            // Remove error messages
            console.log("Valid form");
            imageInput.removeClass("input-error");
            urlErrorText.removeClass("visually-hidden").addClass("visually-hidden");
            uploadErrorText.removeClass("visually-hidden").addClass("visually-hidden");

            // Enable Save button
            saveBtn.removeAttr("disabled");
            saveBtn.ariaInvalid = false;
            return true;
        } else {
            // Disable Save button
            saveBtn.attr("disabled", "disabled");
            saveBtn.ariaInvalid = true;
            return false;
        }
    }
    
    // Validate the Image URL(s) provided by the user
    function validateImageUrl() {
        let imageUrls = imageInput.val().split(",");
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

        if (imageInput.val() === "") {
            validImageUrl = true;
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
        
    }
    
    function validateImageUpload() {
        validImageUpload = imageUpload[0].files.length > 0;
        
        // Check if the user uploaded images with extension .jpg, .jpeg, .png
        let reg = /\.(jpe?g|png)/i;
        for(let i = 0; i < imageUpload[0].files.length; i++) {
            if(imageUpload[0].files[i].name.search(reg) === -1) {
                validImageUpload = false;
            }
        }
        
        // Update messages 
        if (validImageUpload) {
            imageUpload.removeClass("input-error");
            uploadErrorText.removeClass("visually-hidden").addClass("visually-hidden");
        } else {
            // Invalid image url message
            console.log("Error classes should be added now");
            imageUpload.removeClass("input-error").addClass("input-error");
            uploadErrorText.removeClass("visually-hidden");
        }
    }
    
    // Image Url validation on blur and keyup
    imageInput.on("blur keyup", function () {
        validateImageUrl();
        validateImageUrlAndUpload();
    });

    // Image upload validation
    $(document).on("input", "input:file", function () {
        validateImageUpload();
        validateImageUrlAndUpload();
    });
    
    // Check Image on submit - Defensive Programming
    $("#form").submit(function(e) {
       validateImageUrl();
       validateImageUpload();
       let validForm = validateImageUrlAndUpload();
       if(!validForm) {
           e.preventDefault();
       }
    });
});