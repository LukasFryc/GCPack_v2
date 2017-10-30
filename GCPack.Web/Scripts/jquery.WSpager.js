(function ($) {

    var currentPage = 1;
    var pagesCount = 0;
    var rowsPerPage = 10;
    var callbackFunction;
    var itemCount = 0;
    var masterPlugin;
    var EventFromPager = false;
    $.fn.extend({
        //plugin name - animatemenu

        WSPager: function (options) {

            function getParamValue(parramArray, key) {
                var result = '';
                $.each(parramArray, function (index, item) {
                    if (item.split('=')[0] == key) {
                        result = item.split('=')[1];

                    }
                });
                return result;
            }

            var o = options;
            if (o.currentPage == 0 || o.currentPage == '') o.currentPage = 1;


            currentPage = $('#page').val() * 1;
            owner = o.owner;
            itemCount = o.itemCount;
            masterPlugin = o.masterPlugin;

            var pagerHTML = '<div class="WSpager" style="float:right;margin-top:10px" id="pager">';
            pagerHTML += '<img tabIndex="0" alt="To start" class="first" src="/content/images/control_start.png">';
            pagerHTML += '<img tabIndex="0" alt="Previous" class="prev" src="/content/images/control_rewind.png">';
            pagerHTML += '<span  id="currentPage" name="currentPage" style="width:10px;margin-right:2px;">' + currentPage + '</span>/';
            pagerHTML += '<span  id="pagesCount" style="margin-left:2px;"></span>';
            pagerHTML += '<img tabIndex="0" alt="Next" class="next" src="/content/images/control_fastforward.png">';
            pagerHTML += '<img tabIndex="0" alt="To end" class="last" src="/content/images/control_end.png">';
            pagerHTML += '&nbsp;<label><select class="rowsPerPage"><option value="10">10</option><option value="20">20</option><option value="30">30</option><option value="50">50</option><option value="100">100</option><option value="150">150</option><option value="200">200</option><option value="600">600</option></select> na stranku</label>';
            pagerHTML += '</div>';

            $(owner).html(pagerHTML);

            $('.rowsPerPage').val(rowsPerPage);
            rowsPerPage = $('.rowsPerPage').val();
            pagesCount = Math.ceil(itemCount / rowsPerPage);
            $('#pagesCount').html(pagesCount + ' (' + itemCount + ')');

            $('.rowsPerPage').change(function () {
                rowsPerPage = $(this).val();
                
                pagesCount = Math.ceil(itemCount / rowsPerPage);
                $('#pagesCount').html(pagesCount + ' (' + itemCount + ')');
                changePage('first');
            }
			);
            //binding pager arrows
            $(".first").click(function () { changePage('first'); });
            $(".prev").click(function () { changePage('prev'); });
            $(".next").click(function () { changePage('next'); });
            $(".last").click(function () { changePage('last'); });


			$('.WSpager img').bind("keypress" , function(e) {
				if(e.which == 13) {
					$(this).click();
					return false;
				}
			});

            this.SetPager = function (itemsCount) {
                pagesCount = Math.ceil(itemsCount / rowsPerPage);
                $('#pagesCount').html(pagesCount + ' (' + itemsCount + ')');
                if (EventFromPager == false) changePage('first');
                EventFromPager = false;
            }

            // getAllParameters
            this.GetPostData = function () {
            
                return '&PageNum=' + currentPage + '&RowsPerPages=' + rowsPerPage;
            }

			this.setCountItems = function (itemCountNew)
			{
				itemCount = itemCountNew;
				pagesCount = Math.ceil(itemCount / rowsPerPage);
		        $('#pagesCount').html(pagesCount + ' (' + itemCount + ')');
		        
			}


			this.setToFirstPage = function (itemCountNew)
			{
				currentPage = 0;		   
				if (currentPage < 1) currentPage = 1;
				$('#currentPage').html(currentPage);     
			}


            if (o.refreshFirstTime) changePage('first');

            return this;

            function changePage(type) {
                EventFromPager = true;
                currentPage = $('#page').val() * 1;
                var formerCurrentPage = $('#page').val() * 1;
                switch (type) {
                    case 'first':
                        currentPage = 0;
                        break;
                    case 'last':
                        currentPage = pagesCount;
                        break;
                    case 'prev':
                        currentPage -= 1;
                        break;
                    case 'next':
                        currentPage += 1;
                        break;
                }
                if (currentPage > pagesCount) currentPage = pagesCount;
                if (currentPage < 1) currentPage = 1;
                $('#page').val(currentPage);
                $('#currentPage').html(currentPage);
                rowsPerPage = $('.rowsPerPage').val();
                o.fnGetPage();
            }

        }
    });

})(jQuery);