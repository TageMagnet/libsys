<!DOCTYPE html>
<html>

<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <title>upload-form</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="/assets/css/common.css" />
    <link rel="stylesheet" href="/assets/css/upload-form.css" />
    <script src="/assets/js/upload-form.js"></script>

</head>

<body>

    <div class="root">

        <div class="container">

            <div id="drop_area">
                <div class="dotted">
                    <form method="POST">
                        <h3 style="text-align: center">drop files to upload</h3>
                        <input type="file" id="file_input" multiple accept="image/*">
                        <!-- <input type="checkbox" id="is_cover">
                        <label for="is_cover">Is cover image</label> -->
                        <div class="checkbox">
                            <input type="checkbox" id="is_cover">
                            <label for="is_cover">Is cover image</label>
                        </div>
                        <label class="button" for="file_input">Select some files</label>
                        <progress id="progress_bar" max=100 value=0></progress>
                        <span id="progress_counter"></span>
                    </form>
                </div>
                <div id="gallery">

                </div>
            </div>
        </div>

        <div class="container">

        </div>

    </div>
</body>

</html>