function showMdPreview() {
    var text = document.getElementById('sourceMdArea').value,
        target = document.getElementById('targetMdArea'),
        converter = new showdown.Converter(),
        html = converter.makeHtml(text);

    target.innerHTML = html;
}

function showPostContent(content) {
    var text = content,
        target = document.getElementById('targetMdArea'),
        converter = new showdown.Converter(),
        html = converter.makeHtml(text);

    target.innerHTML = html;
}