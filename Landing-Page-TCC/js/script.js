const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registrolink = document.querySelector('.registro-link');
const btnLoginpopup = document.querySelector('.btnLogin-popup');
const iconClose = document.querySelector('.icon-close');

// Carrossel
const carousel = document.getElementById('carousel');
const images = carousel.getElementsByTagName('img');
const prevButton = document.getElementById('prev');
const nextButton = document.getElementById('next');
const indicators = document.querySelectorAll('.carousel-indicators button');
let currentIndex = 0;
let autoSlideInterval;

function toggleModal() {
    const loginBox = document.querySelector('.login');
    const registerBox = document.querySelector('.registro');
    loginBox.classList.toggle('active');
    registerBox.classList.toggle('active');
}



async function handleLogin(event) {
    event.preventDefault(); // Impede o envio do formulário

    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;

    const response = await fetch('http://localhost:5134/api/Auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*'
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
    });

    if (response.ok) {
        const data = await response.json();
        console.log('Login bem-sucedido:', data);

        // Armazenar o token no localStorage ou sessionStorage
        localStorage.setItem('token', data.token);
        localStorage.setItem('username', data.username); // Armazenar o nome de usuário
    
        // Redirecionar para a página inicial (ou outra página)
        window.location.href = 'http://localhost:4200/';  // Exemplo de redirecionamento
    } else {
        const errorData = await response.json();
        console.error('Erro na requisição:', errorData.message);
        alert(errorData.message); // Exibir mensagem de erro ao usuário
    }
    alert('Login bem-sucedido!'); // Mensagem de sucesso
}


async function handleRegister(event) {
    event.preventDefault(); // Impedir que o formulário seja enviado normalmente

    const username = document.getElementById('registerUsername').value;
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;

    const response = await fetch('http://localhost:5134/api/Auth/register', { // URL corrigida para 'Auth/register'
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, email, password })
    });

    if (!response.ok) {
        const errorData = await response.json();
        console.error('Erro na requisição:', response.status, response.statusText, errorData.message);
        alert(errorData.message); // Exibir a mensagem de erro
        return;
    }
    localStorage.setItem('username', username); 
    alert('Registro bem-sucedido! Você pode fazer login agora.'); // Mensagem de sucesso
}

  


// Função para atualizar o carrossel
function updateCarousel() {
    carousel.style.transform = `translateX(-${currentIndex * 100}%)`;
    indicators.forEach((indicator, index) => {
        indicator.classList.toggle('active', index === currentIndex);
    });
}

// Função para ir para o próximo slide
function next() {
    currentIndex = (currentIndex + 1) % images.length;
    updateCarousel();
}

// Função para ir para o slide anterior
function prev() {
    currentIndex = (currentIndex - 1 + images.length) % images.length;
    updateCarousel();
}

// Função para ir a um slide específico
function goToSlide(index) {
    currentIndex = index;
    updateCarousel();
}

// Função para iniciar o carrossel automático
function startAutoSlide() {
    autoSlideInterval = setInterval(next, 3000); // Troca a cada 3 segundos
}

// Função para parar o carrossel automático
function stopAutoSlide() {
    clearInterval(autoSlideInterval);
}

// Adiciona eventos para os botões do carrossel
nextButton.addEventListener('click', () => {
    stopAutoSlide(); // Para o auto slide ao clicar
    next();
    startAutoSlide(); // Reinicia o auto slide
});
prevButton.addEventListener('click', () => {
    stopAutoSlide(); // Para o auto slide ao clicar
    prev();
    startAutoSlide(); // Reinicia o auto slide
});
indicators.forEach((indicator, index) => {
    indicator.addEventListener('click', () => {
        stopAutoSlide(); // Para o auto slide ao clicar
        goToSlide(index);
        startAutoSlide(); // Reinicia o auto slide
    });
});

// Inicializa o carrossel
updateCarousel();
startAutoSlide(); // Inicia o carrossel automático

function toggleModal() {
    wrapper.classList.toggle('active');
}

document.querySelector('.registro-link').addEventListener('click', function() {
    document.querySelector('.login').style.display = 'none';
    document.querySelector('.registro').style.display = 'block';
});

document.querySelector('.login-link').addEventListener('click', function() {
    document.querySelector('.registro').style.display = 'none';
    document.querySelector('.login').style.display = 'block';
});

document.querySelector('.registro').style.display = 'none';

registrolink.addEventListener('click', () => {
    wrapper.classList.add('active');
});
loginlink.addEventListener('click', () => {
    wrapper.classList.remove('active');
});
btnLoginpopup.addEventListener('click', () => {
    wrapper.classList.add('active-popup');
});
iconClose.addEventListener('click', () => {
    wrapper.classList.remove('active-popup');
});