const user_info = document.querySelector(".user-info");
const user_dropdown = document.querySelector(".user_dropdown");
user_info.addEventListener("mouseenter", () => {
  user_dropdown.classList.add("active");
});
user_info.addEventListener("mouseleave", () => {
  user_dropdown.classList.remove("active");
});

