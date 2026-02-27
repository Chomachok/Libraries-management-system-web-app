// Основные скрипты для сайта "Книжная полка"
document.addEventListener('DOMContentLoaded', function() {
    console.log('📖 Книжная полка: загрузка завершена');

    // Подсветка активного пункта меню
    const navItems = document.querySelectorAll('.nav-item');
    const currentPath = window.location.pathname;

    navItems.forEach(item => {
        if (item.getAttribute('href') === currentPath) {
            item.classList.add('active');
        }
    });

    // Клик по карточке (переход на страницу)
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('click', (e) => {
            if (!e.target.closest('a')) {
                const link = card.dataset.url || card.querySelector('a.btn')?.href;
                if (link) window.location = link;
            }
        });
    });
});