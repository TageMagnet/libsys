var title;
var dropArea;
var uploadProgress = [];
var progressBar;
var fileInput;

const initializeProgress = function (numFiles) {
    progressBar.value = 0
    uploadProgress = []

    for (let i = numFiles; i > 0; i--) {
        uploadProgress.push(0)
    }
}

const updateProgress = function (fileNumber, percent) {
    uploadProgress[fileNumber] = percent
    let total = uploadProgress.reduce((tot, curr) => tot + curr, 0) / uploadProgress.length
    console.debug('update', fileNumber, percent, total)
    progress_counter.textContent = `${percent}%`
    progressBar.value = total
}

const highlight = function (ev) {
    dropArea.classList.add('highlight')
}

const unhighlight = function (ev) {
    dropArea.classList.remove('highlight')
}

const preventDefaults = function (ev) {
    ev.preventDefault()
    ev.stopPropagation()
}

const onFileDrop = function (ev) {
    console.log(ev)
}



const previewFile = function (file) {
    let reader = new FileReader()
    reader.readAsDataURL(file)
    reader.onloadend = function () {
        let img = document.createElement('img')
        img.src = reader.result
        document.getElementById('gallery').appendChild(img)
    }
}

const uploadfile = function (file, i) {

    var url = '/file'
    var xhr = new XMLHttpRequest()
    var formData = new FormData()
    xhr.open('POST', url, true)
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest')

    // Update progress (can be used to show progress indicator)
    xhr.upload.addEventListener("progress", function (e) {
        updateProgress(i, (e.loaded * 100.0 / e.total) || 100)
    })

    xhr.addEventListener('readystatechange', function (e) {
        if (xhr.readyState == 4 && xhr.status == 200) {
            updateProgress(i, 100) // <- Add this
        }
        else if (xhr.readyState == 4 && xhr.status != 200) {
            // Error. Inform the user
        }
    })

    xhr.addEventListener('error', function (ev) {
        console.error({ error: ev.error })
    })
    // Cover for existing file
    formData.append('cover', false)
    formData.append('upload_preset', 'ijfgiouahfbnuivboaefh')
    formData.append('file', file)
    console.log({ file: file, i: i, formData: formData })
    xhr.send(formData)
}

const handleFiles = function (files) {
    files = [...files];
    initializeProgress(files.length)

    files.map((file, iterator) => uploadfile(file, iterator))
    //files.forEach(previewFile)
}

const handleDrop = function (ev) {
    var dt = ev.dataTransfer
    var files = dt.files

    handleFiles(files)
}


const postDomLoad = function () {

    dropArea = document.querySelector('#drop_area')
    title = document.querySelector('title')
    progressBar = document.querySelector('#progress_bar')
    fileInput = document.querySelector('#file_input')

    fileInput.addEventListener('change', function (ev) {
        handleFiles(ev.target.files)
    })

    // Change title
    title.innerHTML = String.fromCharCode(10044) + 'upload-file'
    // Reset file input
    fileInput.value = ""

        // Prevent default drag behaviors
        ;['dragenter', 'dragover', 'dragleave', 'drop']
            .map(eventName => {
                window.addEventListener(eventName, preventDefaults, false)
            })

        // Highlight drop area when item is dragged over it
        ;['dragenter', 'dragover'].forEach(eventName => {
            dropArea.addEventListener(eventName, highlight, false)
        })
        ;['dragleave', 'drop'].forEach(eventName => {
            dropArea.addEventListener(eventName, unhighlight, false)
        })

    // Handle dropped files
    dropArea.addEventListener('drop', handleDrop, false)
}





function Rr(f) { /in/.test(document.readyState) ? setTimeout('Rr(' + f + ')', 9) : f() }
Rr(function () {

    postDomLoad()

})/*< --- END READYSTATE />*/