// The Silicon Post - Main JavaScript

document.addEventListener("DOMContentLoaded", function () {
  console.log("The Silicon Post loaded successfully!");

  // Add smooth scrolling to all links
  document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
    anchor.addEventListener("click", function (e) {
      e.preventDefault();
      const target = document.querySelector(this.getAttribute("href"));
      if (target) {
        target.scrollIntoView({
          behavior: "smooth",
        });
      }
    });
  });

  // Mobile menu toggle (if needed in future)
  const mobileMenuToggle = document.querySelector(".mobile-menu-toggle");
  const navMenu = document.querySelector(".nav-menu");

  if (mobileMenuToggle && navMenu) {
    mobileMenuToggle.addEventListener("click", function () {
      navMenu.classList.toggle("active");
    });
  }

  // Add reading time calculator for blog posts
  const postContent = document.querySelector(".post-content");
  if (postContent) {
    const text = postContent.textContent;
    const wordsPerMinute = 200;
    const wordCount = text.trim().split(/\s+/).length;
    const readingTime = Math.ceil(wordCount / wordsPerMinute);

    const postMeta = document.querySelector(".post-meta");
    if (postMeta && readingTime > 0) {
      const readingTimeElement = document.createElement("span");
      readingTimeElement.textContent = `${readingTime} min read`;
      readingTimeElement.className = "reading-time";
      postMeta.appendChild(readingTimeElement);
    }
  }
});
