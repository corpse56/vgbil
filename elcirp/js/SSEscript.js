


function pageLoad() {
    var slide = $find('SlideShowExtender1');
    slide.add_slideChanging(showPageNumber);
    $('img').bind('contextmenu', function(e) {
        return false;
    });

}
//$(document).ready(function() {


    //var slider1 = $find("SlideShowExtender1");
    //slider1.add_slideChanging(showPageNumber);
    //showPageNumber();
    //alert(2);
//});


function showPageNumber() {
    var currentIndex = $find('SlideShowExtender1')._currentIndex + 1;
    var totalCount = $find('SlideShowExtender1')._slides.length;
    var lbl = $get("lblPage");
    lbl.textContent = 'Страница ' + currentIndex + ' из ' + totalCount;
}




/*$(document).ready(function() 
{

    function showPageNumber() {
        var currentIndex = $find('SlideShowExtender1')._currentIndex + 1;
        var totalCount = $find('SlideShowExtender1')._slides.length;
        //alert(totalCount);
        var lbl = $get("lblPage"); //.innerHTML = 'фыва';//document.getElementById('lblPage');//$find("lblPage");$("lblPage");
        //lbl.textContent = 'Страница ' +currentIndex+' из ' +totalCount;
        //alert(lbl);
        lbl.innerText = 'Страница ' + currentIndex + ' из ' + totalCount;
        lbl.textContent = 'Страница ' + currentIndex + ' из ' + totalCount;
        //alert('1');

    }

    $.ajax({
    complete:function() {
    //alert('1');

            var slider1;
            slider1 = $find("SlideShowExtender1");
            slider1.add_slideChanging(showPageNumber);

            var currentIndex = $find('SlideShowExtender1')._currentIndex + 1;
            var totalCount = $find('SlideShowExtender1')._slides.length;
            //alert(totalCount);
            var lbl = $get("lblPage"); //.innerHTML = 'фыва';//document.getElementById('lblPage');//$find("lblPage");$("lblPage");
            //lbl.textContent = 'Страница ' +currentIndex+' из ' +totalCount;
            //alert(lbl);
            lbl.innerText = 'Страница ' + currentIndex + ' из ' + totalCount;
            lbl.textContent = 'Страница ' + currentIndex + ' из ' + totalCount;
            //alert('1');
            
        }
    });
});
*/
    function IncSize()
    {
        var ttable = document.getElementById('targetTable');
        var w = parseInt(ttable.style.width);
        if (w == 200)
        { return;}
        w = w +10;
        ttable.style.width  = w+"%";
        var h = parseInt(ttable.style.height) + 10;
        ttable.style.height  = h+"%";
    }
    function DecSize()
    {
        var ttable = document.getElementById('targetTable');
        var w = parseInt(ttable.style.width);
        if (w == 30)
        { return;}
        w = w - 10;
        ttable.style.width  = w+"%";
        var h = parseInt(ttable.style.height) - 10;
        ttable.style.height  = h+"%";
    }
    function Size100()
    {
        var ttable = document.getElementById('targetTable');
        ttable.style.width  = "100%";
        ttable.style.height  = "100%";
    }
    function gotoPage() 
    {
        var slide = $find('SlideShowExtender1');
        var pageNum = parseInt(document.getElementById("txtPage").value,10);
        var totalCount = $find("SlideShowExtender1")._slides.length;
        if (isNaN(pageNum))
            return;
        if (pageNum <= 0)
            return;    
        if (pageNum > totalCount)
            return;
        slide._currentIndex = pageNum-1;
        slide.setCurrentImage();
    }
    function gpEnter(e) {
        if (e.keyCode == 13) {
            gotoPage();
            return false;
        }
        return true;
    }
    var angle = 0;
    function Rotat() {
        //var images = document.getElementsByTagName('img');

       // document.getElementById("SendA").value = images[0].src;
        //__doPostBack('SendA', '');

        //jQuery("#imgslides").rotate({ angle: 90 });

        var img = document.getElementById('imgslides');
        //document.getElementById('button').onclick = function() {
            angle = (angle + 90) % 360;
            img.className = "rotate" + angle;
        //}
        
        
        //angle = (angle + 90) % 360;
        //jQuery("#container").className = 'rotate180';
        //$("img").find(".myctr").rotate(angle);
        //var imgs = $("img");
       //juhnjjyuhnjyuhjyuhjyuhjuhy var angle = 0;

       // angle += 90;
        //var im = $('#imgslides')[0].id;
        //im.rotate(90);
        //$($('img')[1].id).rotate(angle);
        //alert($($('img')[1].id).id);
            //$("#img")[0].rotate(angle);
        
           // var imgSrcs = [];

            //for (var i = 0; i < imgs.length; i++) {
           //     imgSrcs.push(imgs[i].src);
                //alert(imgSrcs[i]);
           // } 
    }
