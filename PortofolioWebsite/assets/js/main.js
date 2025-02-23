/*=============== MENU ===============*/
const navMenu = document.getElementById('nav-menu'),
    navToggle = document.getElementById('nav-toggle');

/* Menu show - hidden */
navToggle.addEventListener('click', () => {
    navMenu.classList.toggle('show-menu');
    navToggle.classList.toggle(('animate-toggle'));
});

/*=============== REMOVE MENU MOBILE ===============*/
const navLink = document.querySelectorAll('.nav-link');

const navAction = () => {
    const navMenu = document.getElementById('nav-menu');

    navToggle.classList.remove('animate-toggle');
    navMenu.classList.remove('show-menu');
};

navLink.forEach((link) => {
    link.addEventListener('click', navAction);
});
/*=============== CHANGE BACKGROUND HEADER ===============*/
const scrollHeader = () => {
    const header = document.getElementById('header');

    if (this.scrollY >= 20) header.classList.add('bg-header');
    else header.classList.remove('bg-header');
}

window.addEventListener('scroll', scrollHeader);

/*=============== SCROLL SECTIONS ACTIVE LINK ===============*/
const sections = document.querySelectorAll('section[id]');

const scrollActive = () => {
    const scrollY = window.pageYOffset;

    sections.forEach((current) => {
        const sectionHeight = current.offsetHeight,
            sectionTop = current.offsetTop - 58,
            sectionId = current.getAttribute('id'),
            sectionClass = document.querySelector('.nav-menu a[href*=' + sectionId + ']');

        if (scrollY > sectionTop && scrollY <= sectionTop + sectionHeight)
            sectionClass.classList.add('active-link');
        else
            sectionClass.classList.remove('active-link');
    });
};

window.addEventListener('scroll', scrollActive);

/*=============== SERVICES SWIPER ===============*/
var servicesSwiper = new Swiper('.services-swiper', {
    spaceBetween: 32,

    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },

    breakpoints: {
        768: {
            slidesPerView: 2,
        },
        1208: {
            slidesPerView: 3,
        },
    },
});


/*=============== MIXITUP FILTER PORTFOLIO ===============*/
var mixer = mixitup('.work-container', {
    selectors: {
        target: '.mix'
    },
    animation: {
        duration: 300
    }
});


/*=============== PORTOFOLIO SWIPER ===============*/
var portofolioSwiper = new Swiper('.images-swiper', {
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },

    breakpoints: {
        768: {
            slidesPerView: 3,
        },
        1208: {
            slidesPerView: 1,
        },
    },
});

/* Active work */
const linkWork = document.querySelectorAll('.work-item');

function activeWork() {
    linkWork.forEach((link) => {
        link.classList.remove('active-work');
    });

    this.classList.add('active-work');
}

linkWork.forEach((link) => {
    link.addEventListener('click', activeWork);
});

/*=============== RESUME ===============*/
const accordionItems = document.querySelectorAll('.resume-item');

accordionItems.forEach((item) => {
    const header = item.querySelector('.resume-header'),
        content = item.querySelector('.resume-content'),
        icon = item.querySelector('.resume-icon i');

    header.addEventListener('click', () => {
        const isOpen = item.classList.toggle('accordion-open');

        content.style.height = isOpen ? content.scrollHeight + 'px' : 0;
        if (icon) icon.className = isOpen ? 'ri-subtract-line' : 'ri-add-line';

        // accordionItems.forEach((otherItem) => {
        //     if (otherItem !== item && otherItem.classList.contains('accordion-open')) {
        //         otherItem.classList.remove('accordion-open');
        //         otherItem.querySelector('.resume-content').style.height = 0;
        //         otherItem.querySelector('.resume-icon i').className = 'ri-add-line';
        //     }
        // });
    });
});

const accordionItemsInside = document.querySelectorAll('.resume-item-inside');

accordionItemsInside.forEach((insideItem) => {
    const header = insideItem.querySelector('.resume-header'),
        content = insideItem.querySelector('.resume-content'),
        icon = insideItem.querySelector('.resume-icon-inside i');

    header.addEventListener('click', () => {
        const isOpen = insideItem.classList.toggle('accordion-open');

        content.style.height = isOpen ? content.scrollHeight + 'px' : 0;
        if (icon) icon.className = isOpen ? 'ri-subtract-line' : 'ri-add-line';

        setTimeout(() => {
            updateParentHeight(insideItem, isOpen);
        }, 300);

    });
});

function updateParentHeight(element, open) {
    let parent = element.closest('.resume-content');
    if (parent) {
        if (open) {
            parent.style.height = parent.scrollHeight + 'px';
        }
        else {
            var totalHeight = 0;
            accordionItemsInside.forEach((insideItem) => {
                totalHeight += insideItem.scrollHeight;
            });
            parent.style.height = totalHeight + 'px';
        }
    }
}

/*=============== EMAIL JS ===============*/
const contactForm = document.getElementById('contact-form'),
    contactName = document.getElementById('contact-name'),
    contactEmail = document.getElementById('contact-email'),
    contactSubject = document.getElementById('contact-subject'),
    contactMessage = document.getElementById('contact-message'),
    message = document.getElementById('message');

const sendEmail = (e) => {
    e.preventDefault();

    if (contactEmail.value === '' || contactName.value === '' || contactSubject.value === '' || contactMessage.value === '') {
        message.classList.remove('color-first');
        message.classList.add('color-red');
        message.textContent = 'All fields are required';

        setTimeout(() => {
            message.textContent = '';
        }, 3000);
    }
    else {
        emailjs.sendForm('service_j8irqhs', 'template_7fkc70s', '#contact-form', 'xzomb0GzOpJeK0JD2').then(
            () => {
                message.classList.add('color-first');
                message.textContent = 'Message sent ✔';

                setTimeout(() => {
                    message.textContent = '';
                }, 5000);
            },
            (error) => {
                alert('OOPs! SOMETHING WENT WRONG...', error);
            },
        );

        contactEmail.value = '';
        contactName.value = '';
        contactSubject.value = '';
        contactMessage.value = '';
    }
};

contactForm.addEventListener('submit', sendEmail);

/*=============== STYLE SWITCHER ===============*/
const styleSwitcher = document.getElementById('style-switcher'),
    styleSwitcherToggle = document.getElementById('switcher-toggle'),
    styleSwitcherClose = document.getElementById('switcher-close');

/* Switcher show */
styleSwitcherToggle.addEventListener('click', () => {
    styleSwitcher.classList.add('show-switcher');
});

/* Switcher hidden */
styleSwitcherClose.addEventListener('click', () => {
    styleSwitcher.classList.remove('show-switcher');
});

/*=============== THEME COLORS ===============*/
const colors = document.querySelectorAll('.style-switcher-color');

colors.forEach((color) => {
    color.onclick = () => {
        const activeColor = color.style.getPropertyValue('--hue');

        colors.forEach((c) => { c.classList.remove('active-color') });
        color.classList.add('active-color');

        document.documentElement.style.setProperty('--hue', activeColor);
    }
});

/*=============== LIGHT/DARK MODE ===============*/
let currentTheme = 'light';
document.body.className = currentTheme;

document.querySelectorAll('input[name=body-theme]').forEach((input) => {
    input.addEventListener('change', () => {
        currentTheme = input.value;
        document.body.className = currentTheme;
    });
});
