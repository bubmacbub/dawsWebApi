var uri = '/api/NyGovQuery?ou=';




function find() {
    var id = $('#Id').val();
    $.getJSON(uri + id)
        .done(function (data) {
            OnSuccess(data);

        })
        .fail(function (jqXHR, textStatus, err) {
            $('#user').text('Error: ' + err);
        });
}




function OnSearchSuccess(data) {
    console.log(data);
    $('#search').button('reset');
    console.log("Search Container: ");
    console.log($('#searchResultContainer'));
    $('#searchResultContainer').empty();
    var bResp = data.batchResponse;
    var searchResults = bResp.itemsField[0].searchResultEntryField;
    console.log(searchResults);
    $.each(searchResults, function (index, item) {
        var childClass = "row"
        if (index % 2 == 1) {
            childClass = "row well";
        }
        $("<div>").attr("id", "dnContain" + index).attr("class", childClass)
        .appendTo("#searchResultContainer");
        var currentParentDiv = $('#dnContain' + index);
        var longDn = item.dnField;
        var arr = longDn.split(',');
        for (var i = 0; i < arr.length; i++) {

            $("<div>").attr("class", "col-md-2").text(arr[i]).appendTo(currentParentDiv);
        }
        //text("dn:" + item.dnField)

    });
    console.log("Done with search results");

}