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


//category tab
const buttonTab = document.querySelectorAll("[button-tab]");
if (buttonTab.length > 0) {
    buttonTab.forEach(button => {
        button.addEventListener("click", () => {
            const patch = button.getAttribute("data-patch");
            const ariaControls = button.getAttribute("aria-controls");
            fetch(patch, {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "GET"
            })
                .then(res => res.json())
                .then(data => {
                    const tabPane = document.querySelector(`#${ariaControls}`);
                    const container = tabPane.querySelector(".list-product-pro");
                    container.innerHTML = "";
                    data.forEach(product => {
                        const thumbnails = JSON.parse(product.thumbnail);
                        const thumbnailUrl = thumbnails[0];
                        const priceNew = (product.price - (product.price * product.discount / 100)).toLocaleString('en-US', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 2
                        });

                        const price = product.price.toLocaleString('en-US', {
                            minimumFractionDigits: 0,
                            maximumFractionDigits: 2
                        });
                        const item = `
                            <div class="item">
                                <div class="product-image position-relative">
                                    <button class="btn-wishlist">
                                        <i class="fa-regular fa-heart"></i>
                                    </button>
                                    <a href="/product/detail/${product.slug}" class="text-center d-block">
                                        <img src=${thumbnailUrl} alt="img" class="image-product">
                                    </a>
                                    <ul class="option">
                                        <li>
                                            <i class="fa-regular fa-eye"></i>
                                        </li>
                                        <li>
                                            <i class="fa-solid fa-right-left"></i>
                                        </li>
                                    </ul>
                                </div>
                                <div class="product-detail">
                                    <ul class="rating">
                                        <li class="lh-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                 viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                 stroke-linecap="round" stroke-linejoin="round"
                                                 class="feather feather-star fill">
                                                <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2">
                                                </polygon>
                                            </svg>
                                        </li>
                                        <li class="lh-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                 viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                 stroke-linecap="round" stroke-linejoin="round"
                                                 class="feather feather-star fill">
                                                <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2">
                                                </polygon>
                                            </svg>
                                        </li>
                                        <li class="lh-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                 viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                 stroke-linecap="round" stroke-linejoin="round"
                                                 class="feather feather-star fill">
                                                <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2">
                                                </polygon>
                                            </svg>
                                        </li>
                                        <li class="lh-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                 viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                 stroke-linecap="round" stroke-linejoin="round" class="feather feather-star">
                                                <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2">
                                                </polygon>
                                            </svg>
                                        </li>
                                        <li class="lh-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                 viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
                                                 stroke-linecap="round" stroke-linejoin="round" class="feather feather-star">
                                                <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2">
                                                </polygon>
                                            </svg>
                                        </li>
                                    </ul>
                                    <a href="/product/detail/${product.Slug}" class="text-decoration-none">
                                        <h5 class="name">${product.name}</h5>
                                    </a>
                                    <h5 class="price theme-color text-theme">${priceNew} đ
                                        ${product.discount > 0 ? `<del>${price} đ</del>` : ""}
                                        
                                    </h5>
                                    <div class="price-qty">
                                        <div class="counter-number">
                                            <div class="counter">
                                                <div class="qty-left-minus">
                                                    <i class="fa-solid fa-minus"></i>
                                                </div>
                                                <input class="form-control input-number qty-input" type="text"
                                                       name="quantity" value="0">
                                                <div class="qty-right-plus">
                                                    <i class="fa-solid fa-plus"></i>
                                                </div>
                                            </div>
                                        </div>
                                        <button class="buy-pro" button-id="${product.id}">
                                            <svg viewBox="0 0 24 24" width="24" height="24" stroke="currentColor"
                                                 stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round"
                                                 class="css-i6dzq1">
                                                <circle cx="9" cy="21" r="1"></circle>
                                                <circle cx="20" cy="21" r="1"></circle>
                                                <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6">
                                                </path>
                                            </svg>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        `
                        container.insertAdjacentHTML("beforeend", item);
                    })
                })
                .catch(err => console.error("Fetch error:", err));
        })
    })  
}
//end category tab



