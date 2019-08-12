function showImageInfo(fileID) {

    var fileinfo;
    $.ajax({
        url: 'getImageInfo',
        data: {
            fileID: fileID
        },
        contentType: 'application/json; charset=utf-8',
        type: 'GET',
        success: function (data) {
            document.getElementById("filename").innerHTML = data.name;
            document.getElementById("filepath").innerHTML = data.path;
            document.getElementById("filesize").innerHTML = data.size + "kb";
            document.getElementById("fileresolution").innerHTML = data.resolution;
        }
    });
}

function showSoundInfo(fileID) {

    var fileinfo;
    $.ajax({
        url: 'getSoundInfo',
        data: {
            fileID: fileID
        },
        contentType: 'application/json; charset=utf-8',
        type: 'GET',
        success: function (data) {
            document.getElementById("filename").innerHTML = data.name;
            document.getElementById("filepath").innerHTML = data.path;
            document.getElementById("filesize").innerHTML = data.size + "kb";
            document.getElementById("fileduration").innerHTML = data.duration;
            document.getElementById("filecuurpos").innerHTML = data.currpos;
            document.getElementById("filetitle").innerHTML = data.title;
            document.getElementById("fileartist").innerHTML = data.artist;
            document.getElementById("filealbum").innerHTML = data.album;

        }
    });
}

function showVideoInfo(fileID) {

    var fileinfo;
    $.ajax({
        url: 'getVideoInfo',
        data: {
            fileID: fileID
        },
        contentType: 'application/json; charset=utf-8',
        type: 'GET',
        success: function (data) {
            document.getElementById("filename").innerHTML = data.name;
            document.getElementById("filepath").innerHTML = data.path;
            document.getElementById("filesize").innerHTML = data.size + "kb";
            document.getElementById("fileduration").innerHTML = data.duration;
            document.getElementById("filecuurpos").innerHTML = data.currpos;

        }
    });
}