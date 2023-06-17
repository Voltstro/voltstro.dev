import './scss/main.scss';

declare global {
    interface Window {
        blazorHelpers: any;
    }
}

window.blazorHelpers = {
    scrollToFragment: (elementId) => {
        const element = document.getElementById(elementId);

        if (element) {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    },
    highlightCode: async () => {
        const hljs = await import('./highlight');
        void hljs.runHighlight();
    }
}