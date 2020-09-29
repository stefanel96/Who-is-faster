window.BlazorFocusElement = {
    focusInput: function(element) {
        if (element instanceof HTMLElement) {
            element.focus();
        }
    }
};