import hljs from 'highlight.js/lib/common';
import csharp from 'highlight.js/lib/languages/csharp';
import ini from 'highlight.js/lib/languages/ini';
import powershell from 'highlight.js/lib/languages/powershell';

export async function runHighlight(): Promise<void> {
    hljs.registerLanguage('csharp', csharp);
    hljs.registerLanguage('ini', ini);
    hljs.registerLanguage('powershell', powershell);
    hljs.highlightAll();

    //@ts-ignore
    await import('./scss/highlight.scss');
}