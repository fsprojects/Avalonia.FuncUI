window.addEventListener('DOMContentLoaded', () => {
  const navbarmenu = document.querySelector('[data-target=navbarMenu]');
  const menu = document.querySelector('#navbarMenu');
  navbarmenu.addEventListener('click', () => {
    navbarmenu.classList.toggle('is-active');
    menu.classList.toggle('is-active');
  });
});