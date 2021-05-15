$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})


// Anropas när man vill sortera en kolumn i listan av litteratur
function SortBy(sort) {

    //alert('Test: ' + sort);

    document.getElementById('SortBy').value = sort;  
    document.SearchLitteraturForm.submit();
}
