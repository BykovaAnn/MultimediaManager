function getTree() {
    // Some logic to retrieve, or generate tree structure
    var tree = [
  {
      text: "Parent 1",
      nodes: [
        {
            text: "Child 1",
            nodes: [
              {
                  text: "Grandchild 1"
              },
              {
                  text: "Grandchild 2"
              }
            ]
        },
        {
            text: "Child 2"
        }
      ]
  },
  {
      text: "Parent 2"
  },
  {
      text: "Parent 3"
  },
  {
      text: "Parent 4"
  },
  {
      text: "Parent 5"
  }
    ];
    return tree;
}

function getMMTree() {
    var CatalogTree;
    var tree = [];
    $.getJSON('GetFilesTree/', function (data) {
        CatalogTree = data;
        $.each(data, function (i, node) {
            tree[i] = buildTree(node);
        });
        alert(data[0].nodes[0].nodes[0].href);
        return tree;
    });
    var r = 0 + 6;
    return tree;
}

function buildTree(node) {
    var curr_node;
    var children = [];
    if (node.nodes != null) {
        for (var i = 0; i < node.nodes.length; i++) {
            children[i] = buildTree(node.nodes[i]);
        }
        if (node.href == "") {
            curr_node = {
                text: node.text,
                nodes: children
            }
        }
        else {
            curr_node = {
                text: node.text,
                nodes: children,
                href: node.href
            }
        }
    }
    else {
        if (node.href == "") {
            curr_node = {
                text: node.text
            }
        }
        else {
            curr_node = {
                text: node.text,
                href: node.href
            }
        }
    }
    return curr_node;
}

function showInfo(fileID) {
    //if (document.getElementById("filename").innerHTML = "")
    //{
    //    document.getElementById("filename").innerHTML = fileID;
    //    document.getElementById("filepath").innerHTML = fileID;
    //}
    //else
    //{
    //    document.getElementById("filename").innerHTML = ""
    //    document.getElementById("filepath").innerHTML = "";
    //}
    var fileinfo;
    $.ajax({
        url: 'Home/getFileInfo',
        data: {
            fileID: fileID
        },
        contentType: 'application/json; charset=utf-8',
        type: 'GET',
        success: function (data) {
            document.getElementById("filename").innerHTML = data.name;
            document.getElementById("filepath").innerHTML = data.path;
            document.getElementById("filesize").innerHTML = data.size + "kb";
            document.getElementById("fileextension").innerHTML = data.extension;
            document.getElementById("filetype").innerHTML = data.type;
            document.getElementById("filecreated").innerHTML = data.created;
            document.getElementById("filechanged").innerHTML = data.changed;
            document.getElementById("fileadded").innerHTML = data.added;
        }
    });
}
