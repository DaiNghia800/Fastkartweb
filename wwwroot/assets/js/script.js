$('.wrap-notification').slick({
  vertical: true, 
  verticalSwiping: true,
  slidesToShow: 1,
  slidesToScroll: 1,
  autoplay: true,
  autoplaySpeed: 2000,
  arrows: false
});
$('.category-slider').slick({
  slidesToShow: 6,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: $('.home-2 .wrap-btn  .custom.slick-prev-custom'),
  nextArrow: $('.home-2 .wrap-btn .custom.slick-next-custom'),
  responsive: [
    {
      breakpoint: 1080,
      settings: {
        slidesToShow: 5
      }
    },
    {
      breakpoint: 992,
      settings: {
        slidesToShow: 4
      }
    },
    {
      breakpoint: 767,
      settings: {
        slidesToShow: 3
      }
    },
    {
      breakpoint: 575,
      settings: {
        slidesToShow: 2
      }
    }
  ]
});
$('.product-lists').slick({
  slidesToShow:5,
   dots: true,
  infinite: true,
  speed: 500,
  cssEase: 'linear',
   slidesToScroll: 1,
   responsive:[
    {
      breakpoint: 599,
      settings: {
       slidesToShow:2,
        dots: true,
      }
    },
    {
      breakpoint: 992,
      settings: {
       slidesToShow:3,
        dots: true,
      }
    },
     {
      breakpoint: 1200,
      settings: {
       slidesToShow:4,
        dots: true,
      }
    },
   ]
});
$('.slider-faq-box').slick({
  slidesToShow:4,
  dots: false,
  infinite: false,
  speed: 500,
  cssEase: 'linear',
  slidesToScroll: 1,
  responsive: [
    {
      breakpoint: 991,
      settings: {
       slidesToShow:3,
        dots: true,
        autoplay: true,     
        autoplaySpeed: 2000,  
        speed: 800, 
      }
    },
     {
      breakpoint: 767,
      settings: {
       slidesToShow:2,
        dots: true,
        autoplay: true, 
        autoplaySpeed: 2000,
        speed: 800,
      }
    },
    {
      breakpoint: 473,
      settings: {
       slidesToShow:1,
        dots: true,
        autoplay: true, 
        autoplaySpeed: 2000,
        speed: 800,
      }
    },
  ]
});
$('.three-slider').slick({
  slidesToShow: 3,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: $('.home-3 .wrap-btn  .custom.slick-prev-custom'),
  nextArrow: $('.home-3 .wrap-btn .custom.slick-next-custom'),
  responsive: [
    {
      breakpoint: 771,
      settings: {
        slidesToShow: 2
      }
    },
    {
      breakpoint: 578,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.list-hot-deals').slick({
  slidesToShow: 2,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: $('.home-4 .wrap-btn  .custom.slick-prev-custom'),
  nextArrow: $('.home-4 .wrap-btn .custom.slick-next-custom'),
  responsive: [
    {
      breakpoint: 771,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.wrap-pr-first').slick({
  slidesToShow: 1,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: $('.home-7 .wrap-btn  .btn-simp.slick-prev-custom'),
  nextArrow: $('.home-7 .wrap-btn .btn-simp.slick-next-custom'),
});
$('.wrap-pr-second').slick({
  slidesToShow: 1,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: $('.home-7 .wrap-btn.second  .btn-simp.slick-prev-custom'),
  nextArrow: $('.home-7 .wrap-btn.second  .btn-simp.slick-next-custom'),
});
$('.list-blog').slick({
  slidesToShow: 3,
  slidesToScroll: 1,
  responsive: [
    {
      breakpoint: 771,
      settings: {
        slidesToShow: 2
      }
    },
    {
      breakpoint: 578,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.slide-about-2').slick({
  slidesToShow: 3,
  slidesToScroll: 1,
  arrows: false,
  dots: true,
  responsive: [
    {
      breakpoint: 993,
      settings: {
        slidesToShow: 2
      }
    },
    {
      breakpoint: 578,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.slider-user').slick({
  slidesToShow: 3,
  slidesToScroll: 1,
  arrows: false,
  autoplay: true,
  autoplaySpeed: 2000,
  dots: true,
  responsive: [
    {
      breakpoint: 993,
      settings: {
        slidesToShow: 2
      }
    },
    {
      breakpoint: 578,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.slider-5').slick({
  slidesToShow: 4,
  slidesToScroll: 1,
  arrows: false,
  dots: false,
  responsive: [
    {
      breakpoint: 993,
      settings: {
        slidesToShow: 3
      }
    },
    {
      breakpoint: 993,
      settings: {
        slidesToShow: 2
      }
    },
    {
      breakpoint: 500,
      settings: {
        slidesToShow: 1
      }
    },
  ]
});
$('.slider-4-half').slick({
    arrows: false,
    infinite: true,
    slidesToShow: 3,
    slidesToScroll: 1,
    centerMode: true,
    centerPadding: '200px',
    dots: true,
    responsive: [{
        breakpoint: 1524,
        settings: {
            centerPadding: '150px',
        }
    },
    {
        breakpoint: 1291,
        settings: {
            slidesToShow: 2,
            centerPadding: '100px',
        }
    },
    {
        breakpoint: 921,
        settings: {
            slidesToShow: 2,
            centerPadding: '20px',
        }
    },
    {
        breakpoint: 798,
        settings: {
            slidesToShow: 1,
            centerPadding: '50px',
        }
    },
    {
        breakpoint: 798,
        settings: {
            slidesToShow: 1,
            centerPadding: '20px',
        }
    },
    {
        breakpoint: 434,
        settings: {
            slidesToShow: 1,
            centerPadding: '0',
        }
    },
    ]
});
// Search
$(document).ready(function(){
  $('.product-slider').slick({
    slidesToShow: 4,    // Số sản phẩm hiển thị cùng lúc
    slidesToScroll: 3,  // Số sản phẩm trượt mỗi lần click
    // autoplay: true,     // Tự động trượt
    autoplaySpeed: 2000, // Tốc độ trượt (2 giây)
    dots: true,         // Hiển thị dấu chấm điều hướng
    infinite: true,      // Lặp vô hạn

    // Thêm phần responsive
    responsive: [
      {
        breakpoint: 1200, // Màn hình từ 768px đến 992px
        settings: {
          slidesToShow: 4,   // 3 cái
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 992, // Màn hình từ 768px đến 992px
        settings: {
          slidesToShow: 3,   // 3 cái
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 768, // Màn hình nhỏ hơn 768px
        settings: {
          slidesToShow: 2,   // 2 cái
          slidesToScroll: 1
        }
      }
      ,
      {
        breakpoint: 550, // Màn hình nhỏ hơn 768px
        settings: {
          slidesToShow: 1,   // 2 cái
          slidesToScroll: 1
        }
      }
      // Bạn có thể thêm các breakpoint khác ở đây
    ]
  });
});
// End Search
const headerNotification = document.querySelector('.header-notification');
const buttonCloseNotification = document.querySelector('.close-notification');
if(buttonCloseNotification){
  buttonCloseNotification.addEventListener('click', () => {
    headerNotification.style.display = "none";
});
}

// Scroll top 
let mybutton = document.getElementById("back-to-top");
window.onscroll = () => scrollFunction();
const scrollFunction = () => {
  if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
    mybutton.classList.add("show");
  } else {
    mybutton.classList.remove("show");
  }
};

if(mybutton){
  mybutton.addEventListener('click', () => {
  document.body.scrollTop = 0; // For Safari
  document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
});
}



