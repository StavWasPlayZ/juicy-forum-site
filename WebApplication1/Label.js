const requiresLabel = document.querySelectorAll("[label]");
requiresLabel.forEach((labelRequired) => {
    const label = document.createElement("label"), parent = labelRequired.parentElement;

    label.innerText = labelRequired.getAttribute("label");
    label.setAttribute("for", labelRequired.id);
    label.style.marginRight = "3px"
    label.style.fontSize = window.getComputedStyle(parent).fontSize;

    parent.insertBefore(label, labelRequired);


    // Add break if requested
    if (labelRequired.hasAttribute("br"))
        parent.insertBefore(document.createElement("br"), labelRequired);

});