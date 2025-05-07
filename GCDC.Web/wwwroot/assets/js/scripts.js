$(document).ready(function () {
  if ($(".sec-media-gallery-grid").length) {
    initCustomScrollbar();
  }
  if ($("#form-modal-id").length) {
    initCustomScrollbar();
  }
  if ($(".tags_wrapper").length) {
    initFilterCounter();
  }
  if ($(".custom-fancybox").length) {
    initializeFancyboxGallery();
  }
  browserDetect();
  ChangeToSvg();
  opportunitySliderInitialized();
  opportunitySliderDark(".opportunity-slider-dark-1", ".custom-prev-opportunity-dark-1", ".custom-next-opportunity-dark-1", ".current-slide-opportunity-dark-1", ".total-slides-opportunity-dark-1", ".custom-arrows-opportunity-dark-1");
  opportunitySliderDark(".opportunity-slider-dark-2", ".custom-prev-opportunity-dark-2", ".custom-next-opportunity-dark-2", ".current-slide-opportunity-dark-2", ".total-slides-opportunity-dark-2", ".custom-arrows-opportunity-dark-2");
  mediaSlider();
  pressCardSlider();
  mediaGallerySlider();
  whatWeDoSliderInitialized();
  coreCompetenciesSliderInitialized();
  valueSlider();
  sustainabilitySliderV2Initialized();
  sustainabilitySliderV3Initialized();
  sustainabilitySliderInitialized();
  ourObjectiveSliderInitialized();
  initializeTabSliders();
  initCookieBanner();
});
var isRTL = $("html").attr("dir") == "rtl" ? true : false;
var winWidth = $(window).width();
var winHeight = $(window).height();
var navLinks = document.querySelectorAll(".js-navLinks");
$(window).on("resize orientationchange", function () {
  // Do on resize
  winWidth = $(window).width(), winHeight = $(window).height();
  sliderHeight();
});
$(window).on("scroll", function () {});
$(window).on("load", function () {
  setTimeout(function () {
    var overlay = document.querySelector(".animation-overlay");
    if (overlay) {
      overlay.style.height = overlay.scrollHeight + "px"; // Force correct height
    }
    initializeBulletAnimation({
      svgSelector: ".animation-overlay svg",
      pathSelector: "path",
      bulletPointsContainerSelector: ".bullet-points",
      rightBarContentSelector: ".rightBar-content > div",
      investWrapperSelector: ".invest-wrapper",
      bulletSVG: "<svg width=\"18\" height=\"16\" viewBox=\"0 0 18 16\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\n        <path d=\"M18 8.02379C14.433 5.7533 11.3662 3.28006 9.02152 0C6.48963 3.15593 3.71235 5.88777 0 7.97207C3.52627 10.2302 6.61756 12.7003 8.96918 16C11.5034 12.8534 14.3016 10.1195 18 8.02379Z\" fill=\"#CFA474\"/>\n      </svg>"
    });
  }, 500);
  formInitialization();
  fadeAnimation();
  ScrollSmootherInitailize();
  clickVideoPopup();
  dataSrc();
  videoPlayerInit();
  lineRevealAnimation();
  horizontalLineAnimation();
  homePageAnimation();
  scrollTop();
  menuDropdown();
  onMouseHover();
  innerBannerVerticalLine();
  footerVerticalLine();
  initAccordion();
  toggleView();
  initStickyElement();
  initStickyFormTabs();
  animateDiamondOnScroll();
  handleGoogleMapScroll();
  handleIntlInputScroll();
  initAccordionScroll();
  initMarqueeControls();
});
$(document).keyup(function (e) {
  if (e.keyCode == 27) {
    //Do on ESC press
  }
});
var smoother;
function ScrollSmootherInitailize() {
  if (window.matchMedia("(min-width: 1200px)").matches) {
    // Destroy existing smoother instance if it exists
    if (smoother) {
      smoother.kill();
    }
    ScrollTrigger.normalizeScroll(true); // Enable for desktop
    smoother = ScrollSmoother.create({
      smooth: 1.5,
      effects: true,
      type: "touch,wheel,pointer",
      content: ".content"
    });
  } else {
    // Destroy smoother and revert normalization
    if (smoother) {
      smoother.kill();
      smoother = null;
      ScrollTrigger.normalizeScroll(false); // Disable for mobile
    }
  }
}
window.addEventListener("resize", ScrollSmootherInitailize);

// Function to toggle ScrollSmoother pause state
function toggleSmootherPause() {
  if (window.matchMedia("(min-width: 1200px)").matches) {
    if (!smoother) {
      console.error("ScrollSmoother is not initialized");
      return;
    }
    var isPaused = smoother.paused();
    smoother.paused(!isPaused); // Toggle between true and false
    // console.log(isPaused);
  } else {
    document.body.classList.toggle("no-scroll");
    $("html").toggleClass("no-scroll");
    // console.log("no-scroll");
  }
}
function browserDetect() {
  navigator.sayswho = function () {
    var ua = navigator.userAgent,
      tem,
      M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
      tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
      return "IE " + (tem[1] || "");
    }
    if (M[1] === "Chrome") {
      tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
      if (tem != null) return tem.slice(1).join("").replace("OPR", "Opera");
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, "-?"];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
    return M.join(" ");
  }();
  $("body").addClass(navigator.sayswho);
}
function ChangeToSvg() {
  $("img.js-tosvg").each(function () {
    var $img = $(this);
    var imgID = $img.attr("id");
    var imgClass = $img.attr("class");
    var imgURL = $img.attr("src");
    $.get(imgURL, function (data) {
      var $svg = $(data).find("svg");
      if (typeof imgID !== "undefined") {
        $svg = $svg.attr("id", imgID);
      }
      if (typeof imgClass !== "undefined") {
        $svg = $svg.attr("class", imgClass + " insvg");
      }
      $svg = $svg.removeAttr("xmlns:a");
      if (!$svg.attr("viewBox") && $svg.attr("height") && $svg.attr("width")) {
        $svg.attr("viewBox", "0 0 " + $svg.attr("height") + " " + $svg.attr("width"));
      }
      $img.replaceWith($svg);
    }, "xml");
  });
}

// menu
function menuDropdown() {
  if ($(window).width() < 1200) {
    $(".menu-item").off("mouseenter mouseleave").on("click", function (e) {
      e.stopPropagation();
      if ($(this).hasClass("show-menu")) {
        $(this).removeClass("show-menu");
      } else {
        $(".menu-item").removeClass("show-menu");
        $(this).addClass("show-menu");
      }
    });

    // Close menu if clicked outside
    $(document).on("click", function () {
      $(".menu-item").removeClass("show-menu");
    });
  } else {
    $(".menu-item").off("click").hover(function () {
      $(".menu-item").removeClass("show-menu");
      $(this).addClass("show-menu");
    }, function () {
      // Optional: $(this).removeClass("show-menu");
    });
  }
  $(".sec-header").mouseleave(function () {
    $(".menu-item").removeClass("show-menu");
  });
  $(".js-hamburger").on("click", function () {
    $("body").toggleClass("menu--open");
    toggleSmootherPause();
  });
}
var isRTL = $("html").attr("dir") === "rtl";
var translateXValue = isRTL ? "-100%" : "100%";
$(".modal-content").css("transform", "translateX(" + translateXValue + ")");
// function openLeaderModal(imgSrc, title, designation, description) {
//   $("#leader-modal-img").attr("src", imgSrc);
//   $("#leader-modal-title").text(title);
//   $("#leader-modal-designation").text(designation);
//   $("#leader-modal-description").html(description);
//   $("#leader-modal")
//     .addClass("show")
//     .removeClass("hide")
//     .attr("aria-hidden", "false");

//   // Add no-scroll class to body to prevent scrolling
//   $("html").addClass("no-scroll");
//   $("body").addClass("no-scroll");
//   toggleSmootherPause();

//   if (ScrollSmoother.get()) {
//     ScrollSmoother.get().paused(true);
//   }

//   // Check for RTL mode and adjust the transform accordingly
//   var isRTL = $("html").attr("dir") === "rtl";
//   var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

//   $(".modal-content").css("transform", `translateX(${translateXValue})`);

//   // Show modal with animation
//   setTimeout(() => {
//     $(".modal-content").css("transform", "translateX(0)"); // Ensure sliding in
//   }, 10); // Delay to trigger animation
// }

// function closeLeaderModal() {
//   // Check for RTL mode and adjust the transform accordingly
//   var isRTL = $("html").attr("dir") === "rtl";
//   var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

//   $(".modal-content").css("transform", `translateX(${translateXValue})`); // Trigger sliding out

//   // Wait for the animation to complete before removing the modal completely
//   setTimeout(() => {
//     $("#leader-modal").removeClass("show").attr("aria-hidden", "true");

//      // Reset form fields
//      let $form = $("#leader-modal").find("form");
//      if ($form.length) {
//        $form.trigger("reset"); // Reset input fields and textareas
//        $form.find(".form-group").removeClass("success failed"); // Clear validation states
//        $form.find(".char-count").text("0 / 300"); // Reset character count display
//      }

//      // Reset intlTelInput if initialized
//      let phoneInput = $form.find("input[name='phone']")[0];
//      if (phoneInput) {
//        let iti = $form.data("iti");
//        if (iti) iti.setNumber(""); // Reset phone input
//      }

//     // Remove no-scroll class from body to allow scrolling
//     $("html").removeClass("no-scroll");
//     $("body").removeClass("no-scroll");

//     // Resume GSAP ScrollSmoother
//     if (ScrollSmoother.get()) {
//       ScrollSmoother.get().paused(false);
//     }
//   }, 500); // Match the duration of the sliding animation
// }
function openLeaderModal(imgSrc, title, designation, description) {
  $("#leader-modal-img").attr("src", imgSrc);
  $("#leader-modal-title").text(title);
  $("#leader-modal-designation").text(designation);
  $("#leader-modal-description").html(description);
  $("#leader-modal").addClass("show").removeClass("hide").attr("aria-hidden", "false");

  // Add no-scroll class to body to prevent scrolling
  $("html").addClass("no-scroll");
  $("body").addClass("no-scroll");
  toggleSmootherPause();
  if (ScrollSmoother.get()) {
    ScrollSmoother.get().paused(true);
  }

  // Check for RTL mode and adjust the transform accordingly
  var isRTL = $("html").attr("dir") === "rtl";
  var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

  $("#leader-modal > .modal-content").css("transform", "translateX(" + translateXValue + ")");

  // Show modal with animation
  setTimeout(function () {
    $("#leader-modal > .modal-content").css("transform", "translateX(0)"); // Ensure sliding in
  }, 10); // Delay to trigger animation
}
function closeLeaderModal() {
  // Check for RTL mode and adjust the transform accordingly
  var isRTL = $("html").attr("dir") === "rtl";
  var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

  $("#leader-modal > .modal-content").css("transform", "translateX(" + translateXValue + ")"); // Trigger sliding out

  // Wait for the animation to complete before removing the modal completely
  setTimeout(function () {
    $("#leader-modal").removeClass("show").attr("aria-hidden", "true");

    // Reset form fields
    var $form = $("#leader-modal").find("form");
    if ($form.length) {
      $form.trigger("reset"); // Reset input fields and textareas
      $form.find(".form-group").removeClass("success failed"); // Clear validation states
      $form.find(".char-count").text("0 / 300"); // Reset character count display
    }

    // Reset intlTelInput if initialized
    var phoneInput = $form.find("input[name='phone']")[0];
    if (phoneInput) {
      var iti = $form.data("iti");
      if (iti) iti.setNumber(""); // Reset phone input
    }

    // Remove no-scroll class from body to allow scrolling
    $("html").removeClass("no-scroll");
    $("body").removeClass("no-scroll");

    // Resume GSAP ScrollSmoother
    if (ScrollSmoother.get()) {
      ScrollSmoother.get().paused(false);
    }
  }, 500); // Match the duration of the sliding animation
}
$(function () {
  $("body").on("click", ".open-modal-btn", function () {
    openLeaderModal($(this).data("img-src"), $(this).data("title"), $(this).data("designation"), $(this).data("description"));
  }).on("click", ".close", closeLeaderModal).on("click touchstart", function (e) {
    if ($(e.target).is("#leader-modal")) closeLeaderModal();
  });
  $(document).keyup(function (e) {
    return e.key === "Escape" && closeformModal();
  });
});

//form-modal-id
// function openformModal() {
//   // $("#leader-modal-img").attr("src", imgSrc);
//   // $("#leader-modal-title").text(title);
//   // $("#leader-modal-designation").text(designation);
//   // $("#leader-modal-description").html(description);
//   $("#form-modal-id")
//     .addClass("show")
//     .removeClass("hide")
//     .attr("aria-hidden", "false");

//   // Add no-scroll class to body to prevent scrolling
//   $("html").addClass("no-scroll");
//   $("body").addClass("no-scroll");
//   toggleSmootherPause();

//   if (ScrollSmoother.get()) {
//     ScrollSmoother.get().paused(true);
//   }

//   // Check for RTL mode and adjust the transform accordingly
//   var isRTL = $("html").attr("dir") === "rtl";
//   var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

//   $(".modal-content").css("transform", `translateX(${translateXValue})`);

//   // Show modal with animation
//   setTimeout(() => {
//     $(".modal-content").css("transform", "translateX(0)"); // Ensure sliding in
//   }, 10); // Delay to trigger animation
// }

// function closeformModal() {
//   // Check for RTL mode and adjust the transform accordingly
//   var isRTL = $("html").attr("dir") === "rtl";
//   var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

//   $(".modal-content").css("transform", `translateX(${translateXValue})`); // Trigger sliding out

//   // Wait for the animation to complete before removing the modal completely
//   setTimeout(() => {
//     $("#form-modal-id").removeClass("show").attr("aria-hidden", "true");

//      // Reset form fields
//      let $form = $("#form-modal-id").find("form");
//      if ($form.length) {
//        $form.trigger("reset"); // Reset input fields and textareas
//        $form.find(".form-group").removeClass("success failed"); // Clear validation states
//        $form.find(".char-count").text("0 / 300"); // Reset character count display
//      }

//      // Reset intlTelInput if initialized
//      let phoneInput = $form.find("input[name='phone']")[0];
//      if (phoneInput) {
//        let iti = $form.data("iti");
//        if (iti) iti.setNumber(""); // Reset phone input
//      }

//     // Remove no-scroll class from body to allow scrolling
//     $("html").removeClass("no-scroll");
//     $("body").removeClass("no-scroll");

//     // Resume GSAP ScrollSmoother
//     if (ScrollSmoother.get()) {
//       ScrollSmoother.get().paused(false);
//     }
//   }, 500); // Match the duration of the sliding animation
// }
function openformModal() {
  // $("#leader-modal-img").attr("src", imgSrc);
  // $("#leader-modal-title").text(title);
  // $("#leader-modal-designation").text(designation);
  // $("#leader-modal-description").html(description);
  $("#form-modal-id").addClass("show").removeClass("hide").attr("aria-hidden", "false");

  // Add no-scroll class to body to prevent scrolling
  $("html").addClass("no-scroll");
  $("body").addClass("no-scroll");
  toggleSmootherPause();
  if (ScrollSmoother.get()) {
    ScrollSmoother.get().paused(true);
  }

  // Check for RTL mode and adjust the transform accordingly
  var isRTL = $("html").attr("dir") === "rtl";
  var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

  $("#form-modal-id > .modal-content").css("transform", "translateX(" + translateXValue + ")");

  // Show modal with animation
  setTimeout(function () {
    $("#form-modal-id > .modal-content").css("transform", "translateX(0)"); // Ensure sliding in
  }, 10); // Delay to trigger animation
}
function closeformModal() {
  // Check for RTL mode and adjust the transform accordingly
  var isRTL = $("html").attr("dir") === "rtl";
  var translateXValue = isRTL ? "-100%" : "100%"; // Left for RTL, right for LTR

  $("#form-modal-id > .modal-content").css("transform", "translateX(" + translateXValue + ")");

  // Wait for the animation to complete before removing the modal completely
  setTimeout(function () {
    $("#form-modal-id").removeClass("show").attr("aria-hidden", "true");

    // Reset form fields
    var $form = $("#form-modal-id").find("form");
    if ($form.length) {
      $form.trigger("reset"); // Reset input fields and textareas
      $form.find(".form-group").removeClass("success failed"); // Clear validation states
      $form.find(".char-count").text("0 / 300"); // Reset character count display
    }

    // Reset intlTelInput if initialized
    var phoneInput = $form.find("input[name='phone']")[0];
    if (phoneInput) {
      var iti = $form.data("iti");
      if (iti) iti.setNumber(""); // Reset phone input
    }

    // Remove no-scroll class from body to allow scrolling
    $("html").removeClass("no-scroll");
    $("body").removeClass("no-scroll");

    // Resume GSAP ScrollSmoother
    if (ScrollSmoother.get()) {
      ScrollSmoother.get().paused(false);
    }
    resetFormPopup();
  }, 500); // Match the duration of the sliding animation
}
$(function () {
  $("body").on("click", ".open-modal-btn-from", function () {
    openformModal();
  }).on("click", ".close", closeformModal).on("click touchstart", function (e) {
    if ($(e.target).is("#form-modal-id")) closeformModal();
  });
  $(document).keyup(function (e) {
    return e.key === "Escape" && closeLeaderModal();
  });
});

//video
function clickVideoPopup() {
  $("body").on("click", ".js-videoPopup", function () {
    videoPopup($(this));
  }).on("click touchstart", ".parent-cl", function (e) {
    var $videoBox = $(this).closest(".video-box");
    var $video = $videoBox.find("video")[0];
    if (!$video) return; // Ensure video exists

    $videoBox.toggleClass("play-video");
    if (window.innerWidth < 1300) {
      if ($videoBox.hasClass("play-video")) {
        $video.play().catch(function (err) {
          return console.warn("Playback error:", err);
        });
      } else {
        $video.pause();
      }
    }
  });
}
function videoPopup(target) {
  var $target = $(target);
  var videoUrl;
  var vidPlayer = null;
  if (winWidth < 600) {
    videoUrl = $target.data("mobile-url");
  } else {
    videoUrl = $target.data("desktop-url");
  }
  var videoClass = $target.data("video-class");
  var videoWidth = $target.data("width");
  var videoHeight = $target.data("height");
  var videoType = $target.data("video-type");
  var videoPoster = $target.data("video-poster") || null;
  var techOrder = ["html5", "youtube", "wistia"];
  var $content = '<div class="popup-video op-0"><video id="lightboxVideo" width="' + videoWidth + '" height="' + videoHeight + '" preload="auto" controls autoplay class="video-js vjs-layout-large" data-setup="{}"><source src="' + videoUrl + '" type="video/mp4" /><p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="https://videojs.com/html5-video-support/" target="_blank" rel="noopener noreferrer">supports HTML5 video</a></p></video></div>';
  $.fancybox.open({
    type: "html",
    content: $content,
    beforeShow: function beforeShow() {
      $("body").addClass("is--videopopup");
      $(".fancybox-container").addClass(videoClass);
    },
    afterShow: function afterShow() {
      vidPlayer = videojs("lightboxVideo", function () {
        techOrder;
      });
      vidPlayer.src({
        type: videoType === "youtube" ? "video/youtube" : videoType === "wistia" ? "video/wistia" : videoType === "video/mp4" ? "video/mp4" : videoType === "application/x-mpegURL" ? "application/x-mpegURL" : "application/x-mpegURL",
        src: videoType === "youtube" ? "https://www.youtube.com/watch?v=" + videoUrl : videoType === "wistia" ? "http://fast.wistia.com/embed/iframe/" + videoUrl : videoUrl
      });
      if (videoPoster) vidPlayer.poster(videoPoster);
      vidPlayer.on("ready", function () {
        vidPlayer.play();
      });
      $(".popup-video").animate({
        opacity: "1"
      }, 500);
    },
    beforeClose: function beforeClose() {
      $("body").removeClass("is--videopopup");
      videojs("lightboxVideo").dispose();
    }
  });
}
function dataSrc() {
  if (winWidth < 600) {
    $("[data-mobile-src]").each(function () {
      var thisSrc = $(this).attr("data-mobile-src");
      $(this).attr("src", thisSrc);
    });
    $("[data-mobile-poster]").each(function () {
      var thisSrc = $(this).attr("data-mobile-poster");
      $(this).attr("poster", thisSrc);
    });
  } else {
    $("[data-desktop-src]").each(function () {
      var thisSrc = $(this).attr("data-desktop-src");
      $(this).attr("src", thisSrc);
    });
    $("[data-desktop-poster]").each(function () {
      var thisSrc = $(this).attr("data-desktop-poster");
      $(this).attr("poster", thisSrc);
    });
  }
}
function videoPlayerInit() {
  $(".my-video-js").each(function (index) {
    var thisId = $(this).attr("id");
    $(this).addClass("video-js");
    if (!thisId == "") {
      thisId = "video-id-" + index;
      $(this).attr("id", thisId);
    }
    var dataSrc = $(this).data("src");
    if (dataSrc) {
      // Apply data-src to the video src
      $(this).attr("src", dataSrc).removeAttr("data-src");

      // Apply data-src to <source> elements inside the video
      $(this).find("source[data-src]").each(function () {
        $(this).attr("src", $(this).data("src")).removeAttr("data-src");
      });

      // Load the video to apply the sources
      this.load();
    }
    var player = videojs(thisId);
  });
}
function sliderHeight() {
  // Allow DOM changes to settle before calculation
  setTimeout(function () {
    // Select each slider group separately
    $(".js-location-slider, .js-tips-slider, .js-intrest-slider, .js-overnight-slider, .js-venue-slider, .js-staying-slider").each(function () {
      var maxHeight = 0;

      // Find the tallest postBox within the current slider
      $(this).find(".postBox, .card-postBox").each(function () {
        var thisHeight = $(this).outerHeight(); // Get height including padding
        if (thisHeight > maxHeight) {
          maxHeight = thisHeight;
        }
      });
    }, 1000);
  });
}
function fadeAnimation() {
  $("[data-animation-type]").each(function () {
    var element = $(this);
    var delay = element.data("delay") || 0;
    var blur = element.data("blur") || 0;
    var animationType = element.data("animation-type") || "fade-down"; // Default: fade-down

    var animationProps = {};

    // Define animation properties based on the data attribute
    if (animationType === "fade-right") {
      animationProps = {
        opacity: 0,
        x: isRTL ? -200 : 200,
        filter: "blur(" + blur + "px)"
      }; // Move from right
    } else if (animationType === "fade-down") {
      animationProps = {
        opacity: 0,
        y: -5,
        filter: "blur(" + blur + "px)"
      }; // Move from top to bottom
    } else if (animationType === "fade-up") {
      animationProps = {
        opacity: 0,
        y: 50,
        filter: "blur(" + blur + "px)"
      }; // Move from bottom to top
    }
    var tl = gsap.timeline({
      scrollTrigger: {
        trigger: element,
        start: "0% 90%",
        end: "100% 0%",
        toggleActions: "play none none none"
      }
    });

    // Animate the whole element
    tl.fromTo(element, animationProps, {
      opacity: 1,
      x: 0,
      y: 0,
      filter: "blur(0px)",
      duration: 1.2,
      ease: "power2.out",
      delay: delay
    });
  });
}
function lineRevealAnimation() {
  var elements = document.querySelectorAll('[data-animation-scroll="lineReveal"]');
  if (elements && elements.length > 0) {
    $('[data-animation-scroll="lineReveal"]').each(function () {
      var paragraphElement = $(this);
      var delay = $(this).data("delay") || 0;
      var blur = $(this).data("blur") || 0;
      if (!paragraphElement[0]) {
        return;
      }
      paragraphElement.removeClass("opacity-zero");

      // Split text into lines
      var splitText = new SplitText(paragraphElement[0], {
        type: "lines"
      });
      if (!splitText || !splitText.lines) {
        return;
      }
      var lines = splitText.lines;

      // Wrap each line in a div with overflow hidden
      lines.forEach(function (line) {
        if (line && line.parentNode) {
          var wrapper = document.createElement("div");
          wrapper.classList.add("line-wrapper");
          wrapper.style.overflow = "hidden";
          line.parentNode.insertBefore(wrapper, line);
          wrapper.appendChild(line);
        }
      });
      var tl = gsap.timeline({
        scrollTrigger: {
          trigger: paragraphElement,
          start: "0% 90%",
          end: "100% 0%",
          toggleActions: "play none none none"
        }
      });

      // Animate lines with blur and staggered reveal
      if (lines && lines.length > 0) {
        tl.fromTo(lines, {
          opacity: 0,
          y: 100,
          filter: "blur(" + blur + "px)"
        }, {
          opacity: 1,
          y: 0,
          filter: "blur(0px)",
          duration: 1.2,
          ease: "power2.out",
          stagger: 0.2,
          delay: delay
        });
      }
    });
  }
}
function horizontalLineAnimation() {
  if (document.querySelectorAll('[data-animation-scroll="horizontalLine"]')) {
    setTimeout(function () {
      $('[data-animation-scroll="horizontalLine"]').each(function () {
        var lineElement = $(this);
        var delay = lineElement.data("delay") || 0;
        var initialWidth = lineElement.data("initial-width") || 0;
        var tl = gsap.timeline({
          scrollTrigger: {
            trigger: lineElement,
            start: "0% 95%",
            // When the top of the element is 90% from the bottom of the viewport
            end: "100% 0%",
            // End animation when the top of the element reaches the top of the viewport
            toggleActions: "play none none none"
            // markers: true,
          }
        });
        tl.fromTo(lineElement, {
          width: initialWidth + "%"
        }, {
          width: "100%",
          duration: 1,
          ease: "power2.inOut",
          delay: delay
        });

        // Animate diamond icons opacity and position as the line width changes
        tl.fromTo(lineElement.find(".diamond-icon-horizontal"),
        // Find diamond icons inside the element
        {
          opacity: 0
        }, {
          opacity: 1,
          duration: 0.6,
          stagger: 0.4,
          ease: "power2.out"
        }, "<" // Sync with the line animation
        );
      });
    }, 10);
  }
}
function innerBannerVerticalLine() {
  if (document.querySelector(".inner-banner-vertical-line")) {
    gsap.to(".inner-banner-vertical-line", {
      scrollTrigger: {
        trigger: ".inner-banner-vertical-line",
        start: "top 90%",
        toggleActions: "play none none none",
        // Play only once
        onUpdate: function onUpdate() {
          // Animate diamond icons as height changes
          gsap.to(".diamond-icon-vertical", {
            delay: 0.6,
            opacity: 1,
            duration: 1,
            stagger: 0.5,
            ease: "power2.out"
          });
        }
      },
      height: window.matchMedia("(max-width: 600px)").matches ? "9rem" : "19rem",
      duration: 1,
      ease: "power2.in"
    });
  }
}
function footerVerticalLine() {
  if (document.querySelector(".vertical-line-footer")) {
    gsap.to(".vertical-line-footer", {
      scrollTrigger: {
        trigger: ".vertical-line-footer",
        start: "top 90%",
        toggleActions: "play none none none" // Play only once
        // markers: true,
      },
      height: "100%",
      duration: 1,
      ease: "power1.inOut"
    });
  }

  // setInterval(() => {
  //   ScrollTrigger.refresh();
  // }, 500);
}
function observeHeightChange(selector) {
  var element = document.querySelector(selector);
  if (!element) return;
  var resizeObserver = new ResizeObserver(function () {
    ScrollTrigger.refresh();
  });
  resizeObserver.observe(element);
}

// Observe height changes in both sections
observeHeightChange(".faq-sec");
observeHeightChange(".accordion-sec");
function homePageAnimation() {
  // Initial animation to 12rem

  if (document.querySelector(".vertical-line-1")) {
    gsap.to(".vertical-line-1", {
      // height: "12rem",
      height: window.matchMedia("(max-width: 600px)").matches ? "5.5rem" : "12rem",
      duration: 1,
      ease: "power1.inOut",
      onUpdate: function onUpdate() {
        // Animate diamond icons as height changes
        gsap.to(".diamond-icon-vertical", {
          delay: 0.6,
          opacity: 1,
          duration: 1,
          stagger: 0.5,
          ease: "power1.inOut"
        });
      },
      onComplete: function onComplete() {
        // Define responsive behavior
        ScrollTrigger.matchMedia({
          // For mobile devices
          "(max-width: 600px)": function maxWidth600px() {
            gsap.to(".vertical-line-1", {
              height: "10.5rem",
              // Mobile height
              ease: "power2.in",
              duration: 1,
              scrollTrigger: {
                trigger: ".vertical-line-1",
                start: "top 60%",
                // Start when 60% of viewport is reached
                toggleActions: "play none none none" // Play only once
              }
            });
          },
          // For larger screens
          "(min-width: 601px)": function minWidth601px() {
            gsap.to(".vertical-line-1", {
              height: "19rem",
              // Desktop height
              ease: "power2.in",
              duration: 1,
              scrollTrigger: {
                trigger: ".vertical-line-1",
                start: "top 60%",
                // Start when 60% of viewport is reached
                toggleActions: "play none none none" // Play only once
              }
            });
          }
        });
      }
    });
  }
  if (document.querySelector(".vertical-line-2")) {
    gsap.to(".vertical-line-2", {
      height: "6.5rem",
      duration: 2,
      ease: "power1.inOut",
      onComplete: function onComplete() {
        gsap.to(".vertical-line-2", {
          height: "12.875rem",
          ease: "power2.in",
          // delay: 0.2,
          duration: 1,
          scrollTrigger: {
            trigger: ".vertical-line-2",
            start: "top 60%",
            toggleActions: "play none none none" // Play only once
            // markers: true,
          }
        });
      }
    });
  }
  if (document.querySelector(".vertical-line-3")) {
    gsap.to(".vertical-line-3", {
      height: "6.5rem",
      duration: 2,
      ease: "power1.inOut",
      onComplete: function onComplete() {
        gsap.to(".vertical-line-3", {
          height: "8.227rem",
          ease: "power2.in",
          // delay: 0.2,
          duration: 1,
          scrollTrigger: {
            trigger: ".vertical-line-3",
            start: "top 60%",
            toggleActions: "play none none none" // Play only once
            // markers: true,
          }
        });
      }
    });
  }

  //section 1
  if (document.querySelector(".scrolling-text-animation")) {
    var scrollingText = document.querySelector(".scrolling-text-animation");
    scrollingText.classList.remove("opacity-zero");
    if (scrollingText.offsetWidth > window.innerWidth) {
      // Duplicate the content to create an infinite scroll effect
      var textContainer = scrollingText?.parentNode;
      var textClone = scrollingText?.cloneNode(true);
      textContainer?.appendChild(textClone);
      var totalWidth = scrollingText?.offsetWidth;
      var duration1 = 50;
      var tl = gsap.timeline();
      // GSAP timeline for fade-up and marquee animations

      // Initial fade-up animation
      tl.from(scrollingText, {
        opacity: 1,
        y: 350,
        duration: 1.2,
        delay: 1
        // ease: "power2.out",
      });

      // Infinite marquee animation
      tl.to(".scrolling-text-animation", {
        x: isRTL ? "+=" + totalWidth : "-=" + totalWidth,
        //for rtl

        duration: duration1,
        ease: "none",
        delay: 0.5,
        repeat: -1 // Infinite repeat
        // modifiers: {
        //   x: (x) => `${parseFloat(x) % totalWidth}px`, // Reset seamlessly
        // },
      });
    } else {
      var _textContainer = scrollingText.parentNode;
      _textContainer.classList.add("container");

      // Only fade-up animation
      gsap.from(scrollingText, {
        opacity: 1,
        y: 350,
        duration: 1.2,
        delay: 0.2
      });
    }
  }

  // Section 2: Paragraph reveal animation (line-by-line with blur)
  // const splitText = new SplitText(".paragraph-animation", { type: "lines" });
  // const lines = splitText.lines;

  // // Wrap each line in a div with overflow hidden
  // lines.forEach((line) => {
  //   const wrapper = document.createElement("div");
  //   wrapper.classList.add("line-wrapper");
  //   wrapper.style.overflow = "hidden";
  //   line.parentNode.insertBefore(wrapper, line);
  //   wrapper.appendChild(line);
  // });
  // if ($("#section2").length > 0) {
  // const section2 = document.querySelector("#section2");
  // const sectionHeight = section2.offsetHeight;

  // gsap
  //   .timeline({
  //     scrollTrigger: {
  //       trigger: "#section2",
  //       start: "top+=10 bottom",
  //       end: `+=${sectionHeight - 10}`,
  //       scrub: true,
  //       // markers: true,
  //       snap: {
  //         snapTo: 1,
  //         // duration: 1.6,
  //         resistance: 0,
  //         delay: 0,
  //         ease: "none",
  //       },
  //     },
  //   })
  //   .fromTo(
  //     lines,
  //     { opacity: 0, y: 100, filter: "blur(10px)" }, // Start hidden, blurred, and shifted
  //     {
  //       opacity: 1,
  //       y: 0,
  //       filter: "blur(0px)",
  //       duration: 1.5,
  //       ease: "none",
  //       stagger: 0.3,
  //     } // Animate each line sequentially
  //   );
  // }

  // Section 3
  $(".card-container").slick({
    slidesToShow: 5,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 10,
    speed: 5000,
    cssEase: "linear",
    infinite: true,
    arrows: false,
    dots: false,
    pauseOnHover: false,
    // Keep this false to use custom hover handling
    responsive: [{
      breakpoint: 600,
      settings: {
        slidesToShow: 1.5,
        slidesToScroll: 1,
        speed: 5000,
        autoplaySpeed: 0,
        cssEase: "linear"
      }
    }, {
      breakpoint: 1280,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1,
        speed: 5000,
        autoplaySpeed: 0,
        cssEase: "linear"
      }
    }]
  });
  $(".card-container").on("mouseenter", function () {
    $(this).slick("slickPause");
  });
  $(".card-container").on("mouseleave", function () {
    $(this).slick("slickPlay");
  });

  // gsap.timeline({
  //   scrollTrigger: {
  //     trigger: "#section3",
  //     start: "top bottom",
  //     end: "top top",
  //     snap: {
  //       snapTo: 1,
  //       // duration: 0.5,
  //       snapDelay: 0,
  //       resistance: 0,
  //       delay: 0,
  //       ease: "none",
  //     },
  //     // markers: true,
  //   },
  // });
}
$(".sec-interactive-width").find(".img-grid").find(".icon-box img").addClass("js-tosvg");
$(".sec-interactive-width").find(".img-grid").find(".icon-box img").addClass("insvg");
function onMouseHover() {
  var $imgGrids = $(".img-grid");

  // By default, expand the first one and contract others
  $imgGrids.first().addClass("expand");
  $imgGrids.not(":first").addClass("contract");
  $imgGrids.mouseenter(function () {
    $imgGrids.removeClass("expand contract");
    $(this).addClass("expand");
    $imgGrids.not(this).addClass("contract");
  });
  $imgGrids.mouseleave(function () {
    // Keep the first item expanded when no hover
    $imgGrids.removeClass("expand contract");
    $imgGrids.first().addClass("expand");
    $imgGrids.not(":first").addClass("contract");
  });
}
function scrollTop() {
  var lastScrollTop = 0;
  $(window).scroll(function () {
    var st = $(this).scrollTop();
    if (st > lastScrollTop) {
      $("body").removeClass("scrollTop");
      $("body").addClass("bodyScrolled");
    } else {
      $("body").addClass("scrollTop");
      $("body").removeClass("bodyScrolled");
    }
    if (st === 0) {
      $("body").removeClass("scrollTop");
      $("body").removeClass("bodyScrolled");
    }
    lastScrollTop = st;
  });
}

// circle animation

function initializeBulletAnimation(_ref) {
  var svgSelector = _ref.svgSelector,
    pathSelector = _ref.pathSelector,
    bulletPointsContainerSelector = _ref.bulletPointsContainerSelector,
    rightBarContentSelector = _ref.rightBarContentSelector,
    bulletSVG = _ref.bulletSVG,
    investWrapperSelector = _ref.investWrapperSelector;
  var svgElement = document.querySelector(svgSelector);
  var pathElement = svgElement?.querySelector(pathSelector);
  var bulletPointsContainer = document.querySelector(bulletPointsContainerSelector);
  var rightBarContents = document.querySelectorAll(rightBarContentSelector);
  if (!svgElement || !pathElement || !bulletPointsContainer || rightBarContents.length === 0) {
    return;
  }
  gsap.registerPlugin(ScrollTrigger, ScrollToPlugin);
  var totalLength = pathElement.getTotalLength();
  pathElement.style.strokeDasharray = totalLength;
  pathElement.style.strokeDashoffset = totalLength;
  var totalBullets = rightBarContents.length;
  var bullets = [];
  var overlay = document.querySelector(svgSelector);
  if (!overlay) return;
  if (!svgElement) return;
  var viewBox = svgElement.viewBox.baseVal;
  if (!viewBox) return;
  var overlayRect = overlay.getBoundingClientRect();
  var scaleX = overlayRect.width / viewBox.width;
  var scaleY = overlayRect.height / viewBox.height;
  for (var i = 0; i < totalBullets; i++) {
    var position = totalLength - i / (totalBullets - 1) * totalLength;
    var point = pathElement.getPointAtLength(position);
    var bulletX = point.x * scaleX;
    var bulletY = point.y * scaleY;
    var bullet = document.createElement("div");
    bullet.classList.add("bullet");
    bullet.innerHTML = bulletSVG;
    bullet.style.position = "absolute";
    bullet.style.left = bulletX + "px";
    bullet.style.top = bulletY + "px";
    bullet.style.transform = "translate(-50%, -50%)";
    bullet.style.opacity = "0";
    bulletPointsContainer.appendChild(bullet);
    bullets.push(bullet);
  }

  // Set First Bullet & Content as Active (Default)
  function setFirstBulletActive() {
    bullets.forEach(function (bullet, index) {
      bullet.classList.toggle("active", index === 0);
      bullet.style.opacity = index === 0 ? "1" : "0";
    });
    rightBarContents.forEach(function (content, index) {
      if (index === 0) {
        gsap.to(content, {
          opacity: 1,
          y: 0,
          duration: 0.5,
          ease: "slow"
        });
        content.style.width = "auto";
        content.style.height = "auto";
        content.style.visibility = "visible";
        content.classList.add("active");
      } else {
        gsap.to(content, {
          opacity: 0,
          y: 50,
          duration: 0.5,
          ease: "slow"
        });
        content.style.width = "0";
        content.style.height = "0";
        content.style.visibility = "hidden";
        content.classList.remove("active");
      }
    });
  }

  // Default First Bullet & Content Active
  setFirstBulletActive();
  gsap.to(pathElement, {
    strokeDashoffset: 0,
    duration: 0.1,
    ease: "none",
    scrollTrigger: {
      trigger: investWrapperSelector,
      start: "top top",
      end: "+=" + totalLength,
      scrub: 0.5,
      pin: true,
      anticipatePin: 1,
      snap: {
        snapTo: function snapTo(progress) {
          return Math.round(progress * (totalBullets - 1)) / (totalBullets - 1);
        },
        duration: 0.4,
        delay: 0,
        ease: "none"
      },
      onUpdate: function onUpdate(self) {
        var snappedStep = Math.round(self.progress * (totalBullets - 1));
        bullets.forEach(function (bullet, index) {
          bullet.classList.toggle("active", index === snappedStep);
          bullet.style.opacity = index <= snappedStep ? "1" : "0";
        });
        rightBarContents.forEach(function (content, index) {
          if (index === snappedStep) {
            gsap.to(content, {
              opacity: 1,
              y: 0,
              duration: 0.5,
              ease: "slow"
            });
            content.style.width = "auto";
            content.style.height = "auto";
            content.style.visibility = "visible";
            content.classList.add("active");
          } else {
            gsap.to(content, {
              opacity: 0,
              y: 50,
              duration: 0.5,
              ease: "slow"
            });
            content.style.width = "0";
            content.style.height = "0";
            content.style.visibility = "hidden";
            content.classList.remove("active");
          }
        });
      },
      onLeaveBack: function onLeaveBack() {
        gsap.delayedCall(0.3, function () {
          setFirstBulletActive(); //Reset to First Bullet on Scroll Up
          gsap.set(pathElement, {
            strokeDashoffset: totalLength
          });
        });
      }
    }
  });

  // Click Event for Bullets
  bullets.forEach(function (bullet, index) {
    bullet.addEventListener("click", function () {
      var reversedIndex = totalBullets - index - 1;
      var previousIndex = bullets.findIndex(function (b) {
        return b.classList.contains("active");
      });
      var previousReversedIndex = totalBullets - previousIndex - 1;
      var progress = Math.abs(reversedIndex - previousReversedIndex) / (totalBullets - 1);
      var scrollDistance = totalLength * progress;
      gsap.to(window, {
        scrollTo: {
          y: investWrapperSelector,
          offsetY: scrollDistance
        },
        duration: 1,
        ease: "power2.out"
      });
      bullets.forEach(function (b, i) {
        b.classList.toggle("active", i === index);
        b.style.opacity = i <= index ? "1" : "0";
      });
      rightBarContents.forEach(function (content, i) {
        if (i === index) {
          gsap.to(content, {
            opacity: 1,
            y: 0,
            duration: 0.5,
            ease: "slow"
          });
          content.style.width = "auto";
          content.style.height = "auto";
          content.style.visibility = "visible";
          content.classList.add("active");
        } else {
          gsap.to(content, {
            opacity: 0,
            y: 50,
            duration: 0.5,
            ease: "slow"
          });
          content.style.width = "0";
          content.style.height = "0";
          content.style.visibility = "hidden";
          content.classList.remove("active");
        }
      });
    });
  });
}
function valueSlider() {
  function initTabSlider(tabId) {
    var $tabSlider = $("#" + tabId + " .slick-slider");

    // Detect if the page is in RTL mode
    var isRTL = $("html").attr("dir") === "rtl";
    if ($tabSlider.hasClass("slick-initialized")) {
      return;
    }
    $tabSlider.slick({
      infinite: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      speed: 400,
      fade: true,
      prevArrow: $("#" + tabId + " .custom-prev-arrow"),
      nextArrow: $("#" + tabId + " .custom-next-arrow"),
      rtl: isRTL // Enable RTL support if the page is in RTL mode
    });
    var $slides = $tabSlider.find(".slick-slide");
    var totalSlides = $tabSlider.slick("getSlick").slideCount;
    var initialSlideIndex = 0;
    var nextSlideIndex = (initialSlideIndex + 1) % $slides.length;
    updateSlideCount(tabId, 1, totalSlides);
    $slides.removeClass("slick-next");
    $slides.eq(nextSlideIndex).addClass("slick-next");

    // Apply aria-hidden="true" to all slides except the active one
    applySlideAccessibility($tabSlider.slick("getSlick"), initialSlideIndex);

    // Bind the `afterChange` event to this specific slider instance
    $tabSlider.on("afterChange", function (event, slick, currentSlide) {
      var $slides = slick.$slides;
      var nextSlide = (currentSlide + 1) % $slides.length;
      $slides.removeClass("slick-next");
      $slides.eq(nextSlide).addClass("slick-next");

      // console.log(`Next slide index after change: ${nextSlide}`);

      updateSlideCount(tabId, currentSlide + 1, totalSlides);

      // Reapply aria-hidden after the slide change
      applySlideAccessibility(slick, currentSlide);
    });
    function updateSlideCount(tabId, currentSlide, totalSlides) {
      var $slideCount = $("#" + tabId + " .slide-count");
      if (!currentSlide || !totalSlides || totalSlides === 0) {
        $slideCount.html("");
        return;
      }
      var formattedCurrentSlide = currentSlide.toString().padStart(2, "0");
      var formattedTotalSlides = totalSlides.toString().padStart(2, "0");
      $slideCount.html("\n        <span class=\"current-slide heading5\">" + formattedCurrentSlide + "</span>\n        <span class=\"sep-slide heading5\"> / </span>\n        <span class=\"total-slides heading5\">" + formattedTotalSlides + "</span>\n      ");
    }

    // Function to apply aria-hidden="true" on all slides except the active one
    function applySlideAccessibility(slick, currentSlide) {
      var $slides = slick.$slides;

      // Apply aria-hidden="true" on all slides
      $slides.attr("aria-hidden", "true");

      // Remove aria-hidden from the active slide
      $slides.eq(currentSlide).removeAttr("aria-hidden");
    }
  }

  // Initialize the slider for all tabs individually
  $(".tab-content").each(function () {
    var tabId = $(this).attr("id");
    initTabSlider(tabId);
  });
  $("body").on("click", ".tab", function () {
    var targetTab = $(this).data("tab");
    $(".tab").removeClass("active");
    $(".tab-content").removeClass("active").removeClass("flip");
    $(this).addClass("active");
    setTimeout(function () {
      $("#" + targetTab).addClass("active").addClass("flip");
      initTabSlider(targetTab);
    }, 300);
  });
  $("body").on("change", ".dropdown-tabs", function () {
    var targetTab = $(this).val();
    $(".tab").removeClass("active");
    $(".tab-content").removeClass("active").removeClass("flip");
    $(".tab[data-tab='" + targetTab + "']").addClass("active");
    setTimeout(function () {
      $("#" + targetTab).addClass("active").addClass("flip");
      initTabSlider(targetTab);
    }, 300);
  });
  function toggleDropdown() {
    if ($(window).width() <= 600) {
      if ($(".slider-tabs").length > 0 && $(".dropdown-tabs").length === 0) {
        var dropdownHtml = "<select class=\"dropdown-tabs heading4\"> ";
        $(".slider-tabs .tab").each(function () {
          var tabId = $(this).data("tab");
          var tabName = $(this).text();
          dropdownHtml += "<option class=\"heading4\" value=\"" + tabId + "\">" + tabName + "</option>";
        });
        dropdownHtml += "</select>";
        $(".slider-tabs").replaceWith(dropdownHtml);
        $(".dropdown-tabs").select2({
          minimumResultsForSearch: -1,
          dropdownAutoWidth: true,
          width: "auto",
          containerCssClass: "custom-select2-container",
          dropdownCssClass: "custom-select2-dropdown",
          selectionCssClass: "custom-select2-selection",
          templateResult: function templateResult(data) {
            if (!data.id) {
              return data.text;
            }
            return $("<span>" + data.text + "</span>");
          },
          templateSelection: function templateSelection(data) {
            if (!data.id) {
              return data.text;
            }
            var customSelection = "\n              <span>" + data.text + "</span>\n              <div class=\"bottom-active-mob\">\n                <span>\n                  <img class=\"js-tosvg icon-start\" src=\"/assets/img/icon/diamondIconGold.svg\" alt=\"\" />\n                </span>\n                <span>\n                  <img class=\"js-tosvg icon-center\" src=\"/assets/img/icon/diamondIconGold.svg\" alt=\"\" />\n                </span>\n                <span>\n                  <img class=\"js-tosvg icon-end\" src=\"/assets/img/icon/diamondIconGold.svg\" alt=\"\" />\n                </span>\n              </div>\n            ";
            return $(customSelection);
          }
        });
        $(".dropdown-tabs").css("display", "none");
      }
    } else {
      if ($(".dropdown-tabs").length > 0) {
        var tabHtml = "<ul class=\"slider-tabs\">";
        $(".dropdown-tabs option").each(function () {
          var tabId = $(this).val();
          var tabName = $(this).text();
          tabHtml += "<li class=\"tab heading4\" data-tab=\"" + tabId + "\">" + tabName + "</li>";
        });
        tabHtml += "</ul>";
        $(".dropdown-tabs").select2("destroy");
        $(".dropdown-tabs").replaceWith(tabHtml);
      }
    }
  }
  $(window).on("resize", toggleDropdown);
  toggleDropdown();
  $(document).on("click", ".dropdown-tabs", function () {
    $(this).toggleClass("open");
  });
  $(document).on("click", function (e) {
    if (!$(e.target).closest(".dropdown-tabs").length) {
      $(".dropdown-tabs").removeClass("open");
    }
  });
}
function mediaSlider() {
  var $mediaSlider = $(".media-slider");
  $mediaSlider.slick({
    infinite: true,
    slidesToShow: 3,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-media").click(function () {
    $mediaSlider.slick("slickPrev");
  });
  $(".custom-next-media").click(function () {
    $mediaSlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $mediaSlider.slick("getSlick").slideCount;
  $(".total-slides-media").text(formatSlideNumber(totalSlides));
  $(".current-slide-media").text(formatSlideNumber(1));
  $mediaSlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-media").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $mediaSlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-media").hide();
    } else {
      $(".custom-arrows-media").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function pressCardSlider() {
  var $pressCardSlider = $(".press-card-slider");
  $pressCardSlider.slick({
    infinite: true,
    slidesToShow: 3,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-press").click(function () {
    $pressCardSlider.slick("slickPrev");
  });
  $(".custom-next-press").click(function () {
    $pressCardSlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $pressCardSlider.slick("getSlick").slideCount;
  $(".total-slides-press").text(formatSlideNumber(totalSlides));
  $(".current-slide-press").text(formatSlideNumber(1));
  $pressCardSlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-press").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $pressCardSlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-press").hide();
    } else {
      $(".custom-arrows-press").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}

// media gallery slider
function mediaGallerySlider() {
  var $mediaGallerySlider = $(".media-gallery-slider");
  $mediaGallerySlider.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    variableWidth: true,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    },
    // {
    //   breakpoint: 1024,
    //   settings: {
    //     slidesToShow: 1,
    //     slidesToScroll: 1,
    //   },
    // },
    {
      breakpoint: 600,
      settings: {
        infinite: true,
        variableWidth: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });
  $(".custom-prev").click(function () {
    $mediaGallerySlider.slick("slickPrev");
  });
  $(".custom-next").click(function () {
    $mediaGallerySlider.slick("slickNext");
  });
  var totalSlides = $mediaGallerySlider.slick("getSlick").slideCount;
  $(".total-slides").text(formatSlideNumber(totalSlides));
  $(".current-slide").text(formatSlideNumber(1));
  $mediaGallerySlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }
  function toggleCustomArrows() {
    var slickListWidth = 0;
    $mediaGallerySlider.find(".slick-slide").each(function () {
      slickListWidth += Math.round($(this).outerWidth(true)); // Ensure integer values
    });
    var viewportWidth = Math.round($(window).width());
    if (slickListWidth <= viewportWidth) {
      $(".custom-arrows-pagination--gallery").hide();
    } else {
      $(".custom-arrows-pagination--gallery").show();
    }
  }

  // Run initially
  $(document).ready(function () {
    setTimeout(toggleCustomArrows, 300);
  });

  // Update on resize
  $(window).on("resize", function () {
    toggleCustomArrows();
  });

  // Update on Slick events
  $mediaGallerySlider.on("init reInit afterChange", function () {
    toggleCustomArrows();
  });
}
function whatWeDoSliderInitialized() {
  var $whatWeDoSlider = $(".what-we-do-slider");
  $whatWeDoSlider.slick({
    infinite: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 4,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 992,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-what-we-do").click(function () {
    $whatWeDoSlider.slick("slickPrev");
  });
  $(".custom-next-what-we-do").click(function () {
    $whatWeDoSlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $whatWeDoSlider.slick("getSlick").slideCount;
  $(".total-slides-what-we-do").text(formatSlideNumber(totalSlides));
  $(".current-slide-what-we-do").text(formatSlideNumber(1));
  $whatWeDoSlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-what-we-do").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $whatWeDoSlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-pagination").hide();
    } else {
      $(".custom-arrows-pagination").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function coreCompetenciesSliderInitialized() {
  var $whatWeDoSlider = $(".core-competencies-slider");
  $whatWeDoSlider.slick({
    infinite: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 4,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 992,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });
  $(".custom-prev-core").click(function () {
    $whatWeDoSlider.slick("slickPrev");
  });
  $(".custom-next-core").click(function () {
    $whatWeDoSlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $whatWeDoSlider.slick("getSlick").slideCount;
  $(".total-slides-core").text(formatSlideNumber(totalSlides));
  $(".current-slide-core").text(formatSlideNumber(1));
  $whatWeDoSlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-core").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $whatWeDoSlider.find(".slick-track").width();
    var viewportWidth = $(window).width();

    // console.log("check", slickListWidth);
    // console.log("check", viewportWidth);

    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-core").hide();
    } else {
      $(".custom-arrows-core").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function opportunitySliderInitialized() {
  var $opportunitySlider = $(".opportunity-slider");

  // Initialize slick slider for the current slider
  $opportunitySlider.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    // Custom arrows will be used
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 1024,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-opportunity").click(function () {
    $opportunitySlider.slick("slickPrev");
  });
  $(".custom-next-opportunity").click(function () {
    $opportunitySlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $opportunitySlider.slick("getSlick").slideCount;
  $(".total-slides-opportunity").text(formatSlideNumber(totalSlides));
  $(".current-slide-opportunity").text(formatSlideNumber(1));
  $opportunitySlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-opportunity").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $opportunitySlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-opportunity").hide();
    } else {
      $(".custom-arrows-opportunity").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function opportunitySliderDark(sliderSelector, prevArrowSelector, nextArrowSelector, currentSlideSelector, totalSlidesSelector, customArrowsSelector) {
  var $opportunitySlider = $(sliderSelector);
  $opportunitySlider.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 1024,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });
  $(prevArrowSelector).click(function () {
    $opportunitySlider.slick("slickPrev");
  });
  $(nextArrowSelector).click(function () {
    $opportunitySlider.slick("slickNext");
  });

  // Update current and total slides display
  var totalSlides = $opportunitySlider.slick("getSlick").slideCount;
  $(totalSlidesSelector).text(formatSlideNumber(totalSlides));
  $(currentSlideSelector).text(formatSlideNumber(1));
  $opportunitySlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(currentSlideSelector).text(formatSlideNumber(currentSlideIndex));
    toggleCustomArrows(); // Recheck arrow visibility on slide change
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // Arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $opportunitySlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(customArrowsSelector).hide();
    } else {
      $(customArrowsSelector).show();
    }
  }
  toggleCustomArrows();

  // Debounced window resize
  var debounceResize = debounce(function () {
    toggleCustomArrows();
  }, 250);
  $(window).resize(debounceResize);

  // Debounce helper
  function debounce(func, wait) {
    var timeout;
    return function () {
      clearTimeout(timeout);
      timeout = setTimeout(func, wait);
    };
  }
}
function sustainabilitySliderV2Initialized() {
  var $sustainabilitySliderV2 = $(".sustainability-slider-v2");

  // Initialize slick slider for the current slider
  $sustainabilitySliderV2.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    // Custom arrows will be used
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 1024,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-sustainability-v2").click(function () {
    $sustainabilitySliderV2.slick("slickPrev");
  });
  $(".custom-next-sustainability-v2").click(function () {
    $sustainabilitySliderV2.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $sustainabilitySliderV2.slick("getSlick").slideCount;
  $(".total-slides-sustainability-v2").text(formatSlideNumber(totalSlides));
  $(".current-slide-sustainability-v2").text(formatSlideNumber(1));
  $sustainabilitySliderV2.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-sustainability-v2").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $sustainabilitySliderV2.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-sustainability-v2").hide();
    } else {
      $(".custom-arrows-sustainability-v2").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function sustainabilitySliderV3Initialized() {
  var $sustainabilitySliderV3 = $(".sustainability-slider-v3");

  // Initialize slick slider for the current slider
  $sustainabilitySliderV3.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    // Custom arrows will be used
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 1024,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-sustainability-v3").click(function () {
    $sustainabilitySliderV3.slick("slickPrev");
  });
  $(".custom-next-sustainability-v3").click(function () {
    $sustainabilitySliderV3.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $sustainabilitySliderV3.slick("getSlick").slideCount;
  $(".total-slides-sustainability-v3").text(formatSlideNumber(totalSlides));
  $(".current-slide-sustainability-v3").text(formatSlideNumber(1));
  $sustainabilitySliderV3.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-sustainability-v3").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $sustainabilitySliderV3.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-sustainability-v3").hide();
    } else {
      $(".custom-arrows-sustainability-v3").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function sustainabilitySliderInitialized() {
  var $sustainabilitySlider = $(".sustainability-slider");

  // Initialize slick slider for the current slider
  $sustainabilitySlider.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    // Custom arrows will be used
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 1024,
      settings: {
        slidesToShow: 2,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-sustainability").click(function () {
    $sustainabilitySlider.slick("slickPrev");
  });
  $(".custom-next-sustainability").click(function () {
    $sustainabilitySlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $sustainabilitySlider.slick("getSlick").slideCount;
  $(".total-slides-sustainability").text(formatSlideNumber(totalSlides));
  $(".current-slide-sustainability").text(formatSlideNumber(1));
  $sustainabilitySlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-sustainability").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $sustainabilitySlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-sustainability").hide();
    } else {
      $(".custom-arrows-sustainability").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function ourObjectiveSliderInitialized() {
  var $ourObjectiveSlider = $(".our-objective-slider");

  // Initialize slick slider for the current slider
  $ourObjectiveSlider.slick({
    infinite: true,
    slidesToShow: 4,
    slidesToScroll: 1,
    dots: false,
    arrows: false,
    autoplay: false,
    autoplaySpeed: 3000,
    rtl: $("html").attr("dir") === "rtl",
    // Enable RTL based on HTML direction
    responsive: [{
      breakpoint: 1200,
      settings: {
        slidesToShow: 4,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 992,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }, {
      breakpoint: 600,
      settings: {
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
      }
    }]
  });

  // Custom prev and next buttons for What We Do Slider
  $(".custom-prev-our-objective").click(function () {
    $ourObjectiveSlider.slick("slickPrev");
  });
  $(".custom-next-our-objective").click(function () {
    $ourObjectiveSlider.slick("slickNext");
  });

  // Update current and total slides display for What We Do Slider
  var totalSlides = $ourObjectiveSlider.slick("getSlick").slideCount;
  $(".total-slides-our-objective").text(formatSlideNumber(totalSlides));
  $(".current-slide-our-objective").text(formatSlideNumber(1));
  $ourObjectiveSlider.on("afterChange", function (event, slick, currentSlide) {
    var currentSlideIndex = currentSlide + 1;
    $(".current-slide-our-objective").text(formatSlideNumber(currentSlideIndex));
  });
  function formatSlideNumber(slideNumber) {
    return slideNumber < 10 ? "0" + slideNumber : slideNumber;
  }

  // arrow hide function
  function toggleCustomArrows() {
    var slickListWidth = $ourObjectiveSlider.find(".slick-track").width();
    var viewportWidth = $(window).width();
    if (slickListWidth < viewportWidth) {
      $(".custom-arrows-our-objective").hide();
    } else {
      $(".custom-arrows-our-objective").show();
    }
  }
  toggleCustomArrows();
  $(window).resize(function () {
    toggleCustomArrows();
  });
}
function initMarqueeControls() {
  var marquees = document.querySelectorAll(".marquee");
  marquees.forEach(function (marquee) {
    // Remove clones if any (for resize recalculation)
    if (marquee.dataset.cloned === 'true') {
      var allItems = Array.from(marquee.querySelectorAll('.marquee__item'));
      var originalCount = allItems.length / 2;
      var clonedItems = allItems.slice(originalCount);
      clonedItems.forEach(function (item) {
        return item.remove();
      });
      marquee.style.setProperty('--marquee-count', originalCount);
      marquee.dataset.cloned = 'false';
    }
    var promiseElement = document.querySelector(".promise");
    var items = marquee.querySelectorAll(".marquee__item");
    var totalWidth = 0;
    items.forEach(function (item, index) {
      var itemWidth = item.getBoundingClientRect().width;
      totalWidth += itemWidth;
      if (index < items.length - 1) {
        totalWidth += 10; // gap between items
      }
    });
    if (totalWidth > window.innerWidth) {
      // Clone first two items if not cloned already
      if (marquee.dataset.cloned !== "true") {
        var currentCount = parseInt(getComputedStyle(marquee).getPropertyValue("--marquee-count")) || items.length;

        // console.log("currentCount--", currentCount);

        // Clone all items
        items.forEach(function (item, index) {
          var clone = item.cloneNode(true);
          // Assign new --marquee-item-index for cloned items
          clone.style.setProperty("--marquee-item-index", currentCount + index);
          marquee.appendChild(clone);
        });

        // Update --marquee-count (original count + cloned items count)
        marquee.style.setProperty("--marquee-count", currentCount + items.length);
        marquee.dataset.cloned = "true";
      }
      promiseElement.classList.remove("wrapper");

      // Add animation class to all items
      marquee.querySelectorAll(".marquee__item").forEach(function (item) {
        item.classList.add("marquee-animate");
      });
    } else {
      promiseElement.classList.add("wrapper");

      // Remove animation class from all items
      marquee.querySelectorAll(".marquee__item").forEach(function (item) {
        item.classList.remove("marquee-animate");
      });
    }
  });
}
window.addEventListener("resize", initMarqueeControls);
function handleIntlInputScroll() {
  var countryLists = document.querySelectorAll(".iti__country-list");
  if (!countryLists.length) return;
  countryLists.forEach(function (googleMap) {
    googleMap.addEventListener("mouseenter", function () {
      ScrollSmoother.get().paused(true); // Pause GSAP scrolling
    });
    googleMap.addEventListener("mouseleave", function () {
      ScrollSmoother.get().paused(false); // Resume GSAP scrolling
    });
  });
}
function formInitialization() {
  // Tab Switching
  $(".tab-link").click(function () {
    var tabId = $(this).data("tab");
    $(".tab-link").removeClass("active");
    $(this).addClass("active");
    $(".contact-form").removeClass("active");
    $("#" + tabId).addClass("active");
    handleIntlInputScroll();
  });
  $(".contact-form").each(function () {
    var $form = $(this);
    var phoneInput = $form.find("input[name='phone']")[0];
    if (phoneInput) {
      var iti = window.intlTelInput(phoneInput, {
        initialCountry: "sa",
        // Default country
        separateDialCode: true,
        autoPlaceholder: "polite",
        countrySearch: true
      });

      // Store intlTelInput instance in form data
      $form.data("iti", iti);
    }
  });
  $(document).ready(function () {
    function validateField(input) {
      var value = input?.val()?.trim();
      var name = input.attr("name");
      var formGroup = input.closest(".form-group");
      var htmlTagPattern = /<[^>]+>/gi; // Match any HTML tag
      var isValid = true;
      formGroup.removeClass("success failed"); // Reset validation classes

      switch (name) {
        case "phone":
          isValid = value && /^\d+$/.test(value) && value.length >= 2 && value.length <= 15;
          break;
        case "firstName":
        case "lastName":
        case "CompanyName":
        case "JobTitle":
          isValid = value && value.length <= 50 && !htmlTagPattern.test(value);
          break;
        case "email":
          isValid = value && /^\S+@\S+\.\S+$/.test(value);
          break;
        case "message":
          isValid = value && value.length <= 300 && !htmlTagPattern.test(value);
          break;
        default:
          isValid = false;
      }
      if (!isValid) {
        formGroup.addClass("failed");
      } else {
        formGroup.addClass("success");
      }
      return isValid;
    }
    function validateForm($form) {
      var isValid = true;
      $form.find("input, textarea").each(function () {
        if (!validateField($(this))) {
          isValid = false;
        }
      });
      return isValid;
    }
    $("input[type='tel']").on("input", function () {
      $(this).val($(this).val().replace(/\D/g, ""));
    });

    // Validate individual field on input and change
    $(".contact-form").on("input change", "input, textarea", function () {
      validateField($(this));
    });

    // Validate all fields on submit
    $(".contact-form").on("submit", function (event) {
      event.preventDefault();
      var $form = $(this);
      var isValid = validateForm($form);
    });
  });
  $(".contact-form textarea[name='message']").on("input", function () {
    var $form = $(this).closest(".contact-form");
    $form.find(".char-count").text($(this).val().length + " / 300");
  });
}

// card grid View
function activeClassOnlyAll(check) {
  check === "media-gallery" && navLinks.forEach(function (navLink, index) {
    if (index === 0) {
      navLink.classList.add("active");
    } else {
      navLink.classList.remove("active");
    }
  });
}
function toggleView() {
  $(".js-view-type .grid-view-action").on("click", function (event) {
    event.preventDefault();
    activeClassOnlyAll("media-gallery");
    $(this).closest(".js-view-type").find(".grid-view-action").addClass("active");
    $(this).closest(".js-view-type").find(".list-view-action").removeClass("active");
    $(".media-listing").removeClass("list-view-listing").addClass("grid-view-listing");
  });
  $(".js-view-type .list-view-action").on("click", function (event) {
    event.preventDefault();
    activeClassOnlyAll("media-gallery");
    $(this).closest(".js-view-type").find(".list-view-action").addClass("active");
    $(this).closest(".js-view-type").find(".grid-view-action").removeClass("active");
    $(".media-listing").removeClass("grid-view-listing").addClass("list-view-listing");
  });
}
function removeActiveClass() {
  navLinks.forEach(function (navLink) {
    navLink.classList.remove("active");
  });
}
navLinks.forEach(function (navLink) {
  navLink.addEventListener("click", function () {
    removeActiveClass();
    this.classList.add("active");
  });
});
function initStickyElement() {
  if (document.querySelector(".filter-sec") && document.querySelector(".faq-accordion-wrapper")) {
    var startValue = window.innerWidth < 600 ? "top-=0rem top" : "top-=150rem top";
    gsap.to(".filter-sec", {
      scrollTrigger: {
        trigger: ".filter-sec",
        start: startValue,
        endTrigger: ".faq-accordion-wrapper",
        end: "bottom bottom",
        pin: true,
        pinSpacing: false
      }
    });
    if (window.innerWidth < 600) {
      var filterSec = document.querySelector(".filter-sec");
      filterSec.style.transition = "top 0.3s ease";
      var lastScrollTop = 0;
      window.addEventListener("scroll", function () {
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        if (scrollTop > lastScrollTop) {
          // Scrolling down
          filterSec.style.top = "0rem";
        } else {
          // Scrolling up
          filterSec.style.top = "5rem";
        }
        lastScrollTop = scrollTop <= 0 ? 0 : scrollTop;
      });
    }
  }
}
function initStickyFormTabs() {
  // Run this code only if the screen width is below 600px
  if (window.innerWidth < 600) {
    if (document.querySelector(".tabs-nav") && document.querySelector(".contact-tabs")) {
      var startValue = "top-=0rem top"; // Only the small screen value is needed

      gsap.to(".tabs-nav", {
        scrollTrigger: {
          trigger: ".tabs-nav",
          start: startValue,
          endTrigger: ".contact-tabs",
          end: "bottom bottom",
          pin: true,
          pinSpacing: false
        }
      });
      var filterSec = document.querySelector(".tabs-nav");

      // Add CSS transition for top
      filterSec.style.transition = "top 0.3s ease";
      var lastScrollTop = 0;
      window.addEventListener("scroll", function () {
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        if (scrollTop > lastScrollTop) {
          // Scrolling down
          filterSec.style.top = "0rem";
          filterSec.style.background = "#9E7140";
        } else {
          // Scrolling up
          filterSec.style.top = "5.5rem";
          filterSec.style.background = "#9E7140";
        }
        lastScrollTop = scrollTop <= 0 ? 0 : scrollTop;
      });
    }
  }
}
function handleGoogleMapScroll() {
  var googleMap = document.querySelector(".map-box");
  if (!googleMap) return;
  googleMap.addEventListener("wheel", function (event) {
    if (event.ctrlKey) {
      ScrollSmoother.get().paused(true); // Pause GSAP scrolling
    }
  });
  googleMap.addEventListener("mouseleave", function () {
    ScrollSmoother.get().paused(false); // Resume GSAP scrolling
  });

  // Resume scrolling when the user releases the Ctrl key
  document.addEventListener("keyup", function (event) {
    if (event.key === "Control") {
      ScrollSmoother.get().paused(false);
    }
  });
}

// Call the function
handleIntlInputScroll();
function animateDiamondOnScroll() {
  var diamonds = gsap.utils.toArray(".diamond-scroll-anim"); // Select all elements

  if (diamonds.length > 0) {
    diamonds.forEach(function (diamond) {
      gsap.to(diamond, {
        y: "-10rem",
        // Moves up
        ease: "none",
        scrollTrigger: {
          trigger: diamond,
          start: "top bottom",
          end: "bottom top",
          scrub: true
          // markers: true,
        }
      });
    });
  }
}
function initAccordion() {
  var selector = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : ".main-accordion";
  var accordions = document.querySelectorAll(selector);
  if (!accordions.length) {
    return;
  }
  var allAccordionHeaders = document.querySelectorAll(selector + " .accordion-header");
  allAccordionHeaders.forEach(function (item) {
    item.addEventListener("click", function () {
      var isActive = item.classList.contains("active");
      allAccordionHeaders.forEach(function (i) {
        i.classList.remove("active");
        i.nextElementSibling.style.maxHeight = null;
      });
      if (!isActive) {
        item.classList.add("active");
        item.nextElementSibling.style.maxHeight = item.nextElementSibling.scrollHeight + "px";
      }
    });
  });
}

// filter count
function initFilterCounter() {
  var checkboxes = document.querySelectorAll(".tags_wrapper input[type='checkbox']");
    var clearBtn = document.querySelector(".btnWrap .btn");
    var filterCounterValue = document.querySelector(".btnWrap .btn .filterCounterValue");
  function updateCount() {
    var checkedCount = document.querySelectorAll(".tags_wrapper input[type='checkbox']:checked").length;
      filterCounterValue.textContent = checkedCount;
      console.log(checkedCount);
  }
  function clearFilters() {
    checkboxes.forEach(function (checkbox) {
      return checkbox.checked = false;
    });
    updateCount();
  }
  checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener("change", updateCount);
  });
  clearBtn.addEventListener("click", clearFilters);
}

// filter custom scroll bar
function initCustomScrollbar() {
  $(".customScroll_bar").mCustomScrollbar({
    theme: "dark",
    scrollInertia: 500,
    axis: "y",
    mouseWheel: {
      preventDefault: true,
      scrollAmount: 80
    },
    advanced: {
      updateOnContentResize: true
    }
  });
}

// Invest Left content bar padding adjustment behalf of header height

function initHeaderPaddingAdjustment() {
  var leftBarContent = document.querySelector(".invest-sec .invest-wrapper .leftBar-content");
  var header = document.querySelector(".sec-header");
  if (!leftBarContent || !header) return;
  var lastScrollY = window.scrollY;
  function adjustPadding() {
    var screenWidth = window.innerWidth;
    var screenHeight = window.innerHeight;
    if (screenWidth <= 1199) {
      // If width is 1199px and height is 900px, set padding to 1rem
      if (screenWidth === 1199 && screenHeight === 900 || screenWidth === 600) {
        leftBarContent.style.paddingTop = "1rem";
      } else {
        leftBarContent.style.paddingTop = "3.5rem";
      }
      return;
    }
    var headerHeight = header.offsetHeight;
    var headerVisible = window.scrollY < lastScrollY && window.scrollY > 0;
    if (headerVisible) {
      leftBarContent.style.paddingTop = "0";
    } else {
      leftBarContent.style.paddingTop = "0";
    }
    lastScrollY = window.scrollY;
  }
  window.addEventListener("scroll", adjustPadding);
  window.addEventListener("resize", adjustPadding);
  adjustPadding();
}
function initAccordionScroll() {
  var tabs = document.querySelectorAll(".filter-tab");
  var sections = [];

  // Store each tab's target section
  tabs.forEach(function (tab) {
    var targetSelector = tab.getAttribute("data-target");
    var target = document.querySelector(targetSelector);
    if (target) {
      sections.push({
        tab: tab,
        target: target
      });
    }

    // Set up the click event to scroll to the section
    tab.addEventListener("click", function () {
      var targetSelector = this.getAttribute("data-target");
      var target = document.querySelector(targetSelector);
      if (target) {
        // If you're using ScrollSmoother, use ScrollSmoother to handle the scrolling
        var _smoother = ScrollSmoother.get();
        if (_smoother) {
          _smoother.scrollTo(target, true, "top 200px"); // Adjust for offset
        } else {
          // Fallback to normal scroll in case ScrollSmoother is not available
          gsap.to(window, {
            duration: 1,
            scrollTo: {
              y: target,
              offsetY: 100 // Adjust this as needed to account for any sticky headers
            },
            ease: "power2.inOut"
          });
        }
      }

      // Activate the clicked tab
      activateTab(this);
    });
  });

  // Function to activate the current tab
  function activateTab(activeTab) {
    tabs.forEach(function (tab) {
      tab.classList.remove("active"); // Remove active class from all tabs
    });
    activeTab.classList.add("active"); // Add active class to the clicked tab
  }
  sections.forEach(function (_ref2) {
    var tab = _ref2.tab,
      target = _ref2.target;
    ScrollTrigger.create({
      trigger: target,
      start: "top center",
      // Adjust as needed for when to trigger the activation
      end: "bottom center",
      onEnter: function onEnter() {
        return activateTab(tab);
      },
      // Activate the tab when the section enters the viewport
      onEnterBack: function onEnterBack() {
        return activateTab(tab);
      } // Activate the tab when scrolling back up
    });
  });
}

// fancyBox with custom popup

function initializeFancyboxGallery() {
  var galleryItems = $(".custom-fancybox");
  var totalCount = galleryItems.length;
  var currentIndex = 0;
  function animatePopup() {
    var popup = $("#customPopup .popup-media");
    gsap.fromTo(popup, {
      opacity: 0,
      scale: 0.9
    }, {
      opacity: 1,
      scale: 1,
      duration: 0.4,
      ease: "power2.out"
    });
  }
  function updatePopup(index) {
    var item = galleryItems.eq(index);
    var src = item.attr("href"); // This picks the video/image source URL dynamically
    var type = item.attr("data-type"); // Video or image type
    var title = item.attr("data-title") || "Untitled"; // Title of the media
    var date = item.attr("data-date") || "Unknown"; // Date of the media

    var popup = $("#customPopup .popup-media");
    var popupTitle = $("#customPopup .popup-title");
    var popupDate = $("#customPopup .popup-date");
    var popupDownloadButton = $(".popup-download"); // Select the download button

    // Empty the popup content first
    popup.empty();

    // Check if the media type is a video or an image
    if (type === "video") {
      // Create a video element if the type is video
      popup.html("<video controls autoplay><source src=\"" + src + "\" type=\"video/mp4\"></video>");
    } else {
      // Create an image element if the type is image
      popup.html("<img src=\"" + src + "\" alt=\"Gallery Image\">");
    }

    // Update title and date
    popupTitle.text(title);
    popupDate.text(date);

    // Update the download button's href dynamically
    popupDownloadButton.attr("href", src);

    // Update Fancybox index and total count
    var indexElement = $(".fancybox-infobar [data-fancybox-index]");
    var countElement = $(".fancybox-infobar [data-fancybox-count]");
    var formattedIndex = String(index + 1).padStart(2, "0");
    var formattedTotal = String(totalCount).padStart(2, "0");
    indexElement.text("" + formattedIndex);
    countElement.text("" + formattedTotal);

    // Style the index and count
    indexElement.css({
      fontWeight: "400",
      color: "#fff"
    });
    countElement.css({
      fontWeight: "400",
      color: "rgba(255, 255, 255, 0.40)"
    });

    // Animate the popup
    animatePopup();
  }
  function openPopup(index) {
    currentIndex = index;
    $("html").addClass("no-scroll");
    $("body").addClass("no-scroll");
    $.fancybox.open({
      src: "#customPopup",
      type: "inline",
      opts: {
        touch: false,
        animationEffect: "fade",
        transitionEffect: "fade",
        buttons: ["close"],
        beforeShow: function beforeShow() {
          $(".fancybox-infobar [data-fancybox-index]").text(currentIndex + 1);
          $(".fancybox-infobar [data-fancybox-count]").text(totalCount);
          $(document).on("keydown.fancybox", function (e) {
            if (e.key === "Escape") {
              $.fancybox.close();
            }
          });
        },
        afterShow: function afterShow() {
          // toggleSmootherPause();
          if (window.innerWidth > 1199) {
            toggleSmootherPause();
          }
        },
        afterClose: function afterClose() {
          toggleSmootherPause();
          $("html").removeClass("no-scroll");
          $("body").removeClass("no-scroll");
          $(document).off("keydown.fancybox");
        }
      }
    });
    updatePopup(currentIndex);
  }

  // Click event for the gallery items
  $(".custom-fancybox").on("click", function (e) {
    e.preventDefault();
    var index = galleryItems.index(this);
    openPopup(index);
  });

  // Navigation for next and previous buttons
  $("#prev-btn").on("click", function () {
    currentIndex = (currentIndex - 1 + totalCount) % totalCount;
    updatePopup(currentIndex);
  });
  $("#next-btn").on("click", function () {
    currentIndex = (currentIndex + 1) % totalCount;
    updatePopup(currentIndex);
  });

  // Set total count in the Fancybox infobar
  $(".fancybox-infobar [data-fancybox-count]").text(totalCount);
}
function initializeTabSliders() {
  // Function to initialize the slider for a specific tab set
  function initSlider(container, tabId) {
    var $textSlider = $(container).find(tabId + " .text-slider");
    var $imageSlider = $(container).find(tabId + " .image-slider");
    var $sliderCounter = $(container).find(tabId + " .slider-counter"); // Element to display slide count

    // Check if the document is in RTL mode
    var isRtl = $("html").attr("dir") === "rtl";

    // Check if the slider is already initialized
    if ($textSlider.hasClass("slick-initialized")) {
      $textSlider.slick("unslick"); // Destroy the slider if it exists
      $imageSlider.slick("unslick"); // Destroy image slider if it exists
    }

    // Check the number of slides
    var textSlideCount = $textSlider.children().length;
    var imageSlideCount = $imageSlider.children().length;

    // If more than one slide, enable arrows; else, hide arrows
    var arrowsEnabled = textSlideCount > 1;

    // Re-initialize both sliders
    $textSlider.slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      asNavFor: $imageSlider,
      // Sync with image slider
      fade: true,
      arrows: arrowsEnabled,
      // Enable arrows only if more than 1 slide
      prevArrow: $(container).find(tabId + " .slick-prev"),
      nextArrow: $(container).find(tabId + " .slick-next"),
      adaptiveHeight: true,
      swipe: true,
      // Enable swipe functionality
      touchThreshold: 10,
      // Touch sensitivity
      infinite: true,
      // Avoid looping unless needed
      rtl: isRtl // Set RTL mode based on document's direction
    });
    $imageSlider.slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      asNavFor: $textSlider,
      // Sync with text slider
      fade: false,
      arrows: false,
      // No arrows for image slider
      swipe: true,
      // Enable swipe
      touchThreshold: 10,
      infinite: true,
      rtl: isRtl // Set RTL mode for image slider
    });

    // Hide custom arrows if only one slide
    if (!arrowsEnabled) {
      $(container).find(".custom-arrow").hide();
    } else {
      $(container).find(".custom-arrow").show();
    }

    // Update slide counter on initialization
    updateSlideCounter(1, textSlideCount);

    // Update slide counter on slide change
    $textSlider.on("afterChange", function (event, slick, currentSlide) {
      var currentIndex = currentSlide + 1; // Slick currentSlide is zero-based
      updateSlideCounter(currentIndex, textSlideCount);
    });

    // Function to update the slide counter
    function updateSlideCounter(currentSlide, totalSlides) {
      var formattedCurrent = ("0" + currentSlide).slice(-2); // Add leading zero
      var formattedTotal = ("0" + totalSlides).slice(-2); // Add leading zero
      // Update slider counter with separate classes for current and total
      $sliderCounter.html("\n        <span class=\"current-slide heading5\">" + formattedCurrent + "</span> /\n        <span class=\"total-slides heading5\">" + formattedTotal + "</span>\n      ");
    }
  }

  // Function to handle tab switching for a specific tab set
  function handleTabSwitch(container) {
    $(container).find(".tab").on("click", function () {
      var targetTab = $(this).data("tab");

      // Remove active class from all tabs and panes within this container
      $(container).find(".tab, .tab-pane").removeClass("active");

      // Add active class to clicked tab and corresponding pane
      $(this).addClass("active");
      $(container).find("#" + targetTab).addClass("active");

      // Initialize the slider in the active tab each time you visit
      initSlider(container, "#" + targetTab);
    });

    // Initialize slider for the first tab on page load
    var firstActiveTab = $(container).find(".tab.active").data("tab") || "tab-1";
    initSlider(container, "#" + firstActiveTab); // Make sure first tab slider is initialized
  }

  // Call the function for each tab container on the page
  $(".tabs-wrapper").each(function () {
    handleTabSwitch(this); // Initialize the tab switching functionality
  });
}

// Ensure that this function runs on page load
$(document).ready(function () {
  initializeTabSliders();
});
$(".js--close").on("click", function (e) {
  $(".form--wrapper").removeClass("formSuccess");
});
$(".js--close2").on("click", function (e) {
  $(".form--wrapper").removeClass("formfailed");
});
$(document).ready(function () {
  $("a[data-page]").on("click", function (e) {
    e.preventDefault();
    var page = $(this).data("page");
    var section = $(this).data("section");

    // Check if the user is already on the target page
    if (window.location.pathname.includes(page)) {
      // Same page, smooth scroll
      var target = $("#" + section);
      if (target.length) {
        $("html, body").animate({
          scrollTop: target.offset().top
        }, 800);
      }
    } else {
      // Different page, navigate with hash
      window.location.href = page + "#" + section;
    }
  });

  // Check for hash in URL and smooth scroll on page load
  if (window.location.hash) {
    var target = $(window.location.hash);
    if (target.length) {
      $("html, body").animate({
        scrollTop: target.offset().top
      }, 800);
    }
  }
});
function initCookieBanner() {
  var cookieBanner = $("#cookieBanner");
  var cookieConsent = localStorage.getItem("cookieConsent");

  // Correct comparison using string
  if (cookieConsent !== "1") {
    cookieBanner.removeClass("hide"); // Remove 'hide' class to show banner
  }
  $("#acceptCookies").on("click", function () {
    localStorage.setItem("cookieConsent", "1"); // accepted
    cookieBanner.fadeOut();
  });
  $("#declineCookies").on("click", function () {
    localStorage.setItem("cookieConsent", "0"); // declined
    cookieBanner.fadeOut();
  });
}