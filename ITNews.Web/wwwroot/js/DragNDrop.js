class DragNDrop {
    constructor(containerId) {
        this.containerId = containerId;
        this.messageBoxId = 'ContentMessageBox';

        var str = '<p id="ContentMessageBox"></p>';
        $(str).insertAfter('#' + this.containerId);
    }

    create() {
        var dropZone = $('#' + this.containerId),
            messageBox = $('#' + this.messageBoxId),
            maxFileSize = 1000000; // max size- 1mb.

        //check browser
        if (typeof (window.FileReader) === 'undefined') {
            messageBox.text('Not supported by browser!');
        }

        dropZone[0].ondrop = function (event) {
            event.preventDefault();
            var file = event.dataTransfer.files[0];

            // check file size
            if (file.size > maxFileSize) {
                messageBox.text('File is too big!');
                return false;
            }

            var imageTypes = ['image/png', 'image/gif', 'image/bmp', 'image/jpg', 'image/jpeg'];

            if (!imageTypes.includes(file.type)) {
                messageBox.text('This isn\'t image !');
                return false;
            }

            var data = new FormData();
            data.append("file", file);
            $.ajax({
                type: "POST",
                url: '/Post/SaveFile',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    dropZone.val(dropZone.val() + '![](' + result + ')');
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        };

        function stateChange(event) {
            if (event.target.readyState === 4) {
                if (event.target.status === 200) {
                    dropZone.val(dropZone.val() + '![](' + this.responseText.replace(/^"(.+(?="$))"$/, '$1') + ')');
                } else {
                    messageBox.text('Error!');
                }
            }
        }

    }
}