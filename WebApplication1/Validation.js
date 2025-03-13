const form = document.querySelector("form"), errorBox = form.querySelector("#message"), inputs = form.querySelectorAll("input:not([type=hidden])");

/**
    * selectors: A query to identify the field(s)
    * maxLength, minLength: Self explenatory
    * prefix: How should the error message refer to the field(s)?
    * check: The validation function to run. Must return true/false
    * 
    * onlySignup, onlyEdit: Self explenatory
    * allowEmpty: Self explenatory. Only works on non-edit forms.
*/
const checks = [
    {
        selectors: "#password",
        maxLength: 64,
        minLength: 3,
        prefix: "A password",
        check: function (password, loggingIn) {
            // SIGNUP ONLY: Check if passwords match
            if (!loggingIn && (password !== form.querySelector("#cPassword").value)) {
                throwError("Passwords don't match");
                return false;
            }

            let capital = false, numeric = false;
            for (let i = 0; i < password.length; i++) {
                const currChar = password.charAt(i),
                    // Is the character alphabetic | numeric?
                    isAlphabetic = isBetween('a', 'z', currChar), isNumeric = isBetween('1', '9', currChar);

                // If neither
                if (!(isAlphabetic || isNumeric)) {
                    throwError(
                        "One's password may only contain these of the English alphabet and the modern numeric system. (A-Z, 1-9)"
                    );
                    return false;
                }

                // On sign up-
                if (!loggingIn) {
                    // -mark whether the password is valid, according to above variables
                    if (!capital && isAlphabetic && (currChar.toUpperCase() === currChar))
                        capital = true;
                    else if (isNumeric)
                        numeric = true;
                }

            }

            if (!loggingIn) {
                if (!capital) {
                    throwError("A password must include at least a single capital letter.");
                    return false;
                }
                if (!numeric) {
                    throwError("A password must include at least a single numeric number.");
                    return false;
                }
            }

            return true;
        }
    },
    {
        selectors: "#email",
        maxLength: 345,
        minLength: 5,
        prefix: "An email",
        /*
            Since input type 'email' already takes care of email validation, there is not much need for it here.
            If a user goes out of their way to mess with the 'input' tag in the DOM, what stops them from disabling the whole validation function?
            It is much more relevant to have this check server-side, but since we don't even send any emails anywhere, it's just not that ideal.
        
            Besides, there are a few more things to take into consideration when validating an email address than studied in class.
            See: https://help.returnpath.com/hc/en-us/articles/220560587-What-are-the-rules-for-email-address-syntax
            Not impossible, just more hassle than necessary.
        */
    },

    // SIGNUP CHECKS
    // Names may not require a special treatment: Hebrew, English, speical chracters (Renée, Adrián, מישהו, Путин, etc)...
    // Other than symbols such as @, ! which are not allowed
    {
        selectors: "#fname,#lname",
        maxLength: 18,
        minLength: 3,
        onlySignUp: true,
        prefix: "A name",
        check: (name) => nameCheck(name)
    },
    {
        selectors: "#birthdate",
        maxLength: 18,
        minLength: 3,
        onlySignUp: true,
        prefix: "A name",
        check: () => {
            const subTime = new Date().getTime() - checkedElement.valueAsDate.getTime();
            if (subTime < 0) {
                throwError("A child born from the future, I sense? How's the weather out there?");
                return false;
            }
            if (subTime > 3.156e+12) {
                throwError("People who are older than 100 years are not considered cool enough for this website. Sorry, but not sorry.");
                return false;
            }
            if (subTime < 4.102e+11) {
                throwError("This website may only be accessed by those who's age is above 13. We do not deal with COPPA.<br/>Just go play Fortnite or something, kiddo");
                return false;
            }
            return true;
        }
    },

    // Profile Checks
    {
        selectors: "#uname",
        maxLength: 20,
        minLength: 3,
        // Things that are on sign up are always going to be on the edit page either way
        onlySignUp: true,
        // Because sign up
        allowEmpty: true,
        noBrackets: true,
        prefix: "A name"
    },

    {
        selectors: "#description",
        maxLength: 345,
        onlyEdit: true,
        noBrackets: true,
        prefix: "A description"
    },

    {
        selectors: "#link1N,#link2N,#link3N",
        maxLength: 20,
        // May be difficult to click
        minLength: 3,
        onlyEdit: true,
        noBrackets: true,
        prefix: "A name"
    },
    {
        selectors: "#link1,#link2,#link3",
        maxLength: 100,
        onlyEdit: true,
        noBrackets: true,
        prefix: "A link",
        // I won't be checking for a name pair since it's quite obvious that it mustn't be empty
        check: (link) => linkCheck(link)
    },
    {
        selectors: "#pfpURL",
        maxLength: 4000,
        onlyEdit: true,
        prefix: "A link",
        check: (link) => {
            if (!linkCheck(link))
                return false;


            for (let i = 0; i < supportedImgTypes.length; i++)
                if (link.endsWith("." + supportedImgTypes[i]))
                    return true;

            throwError("A link must direct to an image URL");
            return false;
        }
    }
]
const supportedImgTypes = [
    "jpeg", "jpg", "gif", "png", "svg", "png"
]



/**
    * min & max both in lower case
    * @param {string} character
    */
function isBetween(min, max, character) {
    const char = character.toLowerCase();
    return (min <= char) && (char <= max);
}
function isSpecial(char, notInclude) {
    // According to Unicode table
    if (isBetween(' ', '/', char) || isBetween(':', '@', char) || isBetween('{', '~', char) || isBetween('¡', '¿', char)) {
        if (notInclude != undefined)
            for (let i = 0; i < notInclude.length; i++)
                if (notInclude[i] == char)
                    return false;
        return true;
    }
    return false;
}

function nameCheck(name) {
    for (let i = 0; i < name.length; i++)
        // Sha'al, Elizabeth the Queen #2?, "Robert" Robenson??
        if (isSpecial(name[i], ["'", "#", '"', " "])) {
            throwError("Invalid name entered");
            return false;
        }
    return true;
}
/**
    Validating a URL is more than just a hassle.
The best approach for it would be using a Regex, since we'll just have way too many checks and possibilities.
Here is how complex the Regex may get:
"^(?:(?:http(?:s)?|ftp)://)(?:\\S+(?::(?:\\S)*)?@)?(?:(?:[a-z0-9\u00a1-\uffff](?:-)*)*(?:[a-z0-9\u00a1-\uffff])+)(?:\\.(?:[a-z0-9\u00a1-\uffff](?:-)*)*(?:[a-z0-9\u00a1-\uffff])+)*(?:\\.(?:[a-z0-9\u00a1-\uffff]){2,})(?::(?:\\d){2,5})?(?:/(?:\\S)*)?$"
(Source: https://cran.r-project.org/web/packages/rex/vignettes/url_parsing.html)
Although it includes non-web services, we must take into account that an address can either start with ww1, www, https://, http://...
We also need to consider restricted characters, subdomains, etc.
Assuming there are only but a few options is not a very good practice.

I won't be pretending like I know how to write such complex Regex-es, nor do I understand nearly a thing from it,
but I still need a way to verify that links won't lead to a dead end.
Hence, I will not be writing my own check system, but instead use said Regex.

(P.S: It doesn't take localhost into account, but doubt it will matter since I'm not saving it to the server)
*/
/** Checks a given link */
function linkCheck(link) {
    if (link.match(
        "^(?:(?:http(?:s)?|ftp)://)(?:\\S+(?::(?:\\S)*)?@)?(?:(?:[a-z0-9\u00a1-\uffff](?:-)*)*(?:[a-z0-9\u00a1-\uffff])+)(?:\\.(?:[a-z0-9\u00a1-\uffff](?:-)*)*(?:[a-z0-9\u00a1-\uffff])+)*(?:\\.(?:[a-z0-9\u00a1-\uffff]){2,})(?::(?:\\d){2,5})?(?:/(?:\\S)*)?$"
    )) return true;

    throwError("A link must be... a link.");
    return false;
}

/** @type {HTMLInputElement} */
let checkedElement
let prevErrElement, editing;
function throwError(errorMsg) {
    errorBox.innerHTML = errorMsg;

    checkedElement.style.borderColor = "red";
    checkedElement.focus();

    if (editing)
        select(aFields.indexOf(checkedElement.parentElement));

    if (prevErrElement != checkedElement) {
        if (prevErrElement != undefined)
            prevErrElement.style.border = "";
        prevErrElement = checkedElement;
    }
}


function validate(formType) {
    const loggingIn = formType == "login";
    // No signing since already signed (before throwError(errorMsg))
    editing = formType == "edit";

    for (let i = 0; i < checks.length; i++) {
        const checkV = checks[i];

        // Check if the object is related to the form (ignore editing for onlySignup)
        if ((checkV.onlySignUp && loggingIn) || (checkV.onlyEdit && !editing))
            continue;

        // Query the field(s) from the object
        const elements = form.querySelectorAll(checkV.selectors);
        for (let j = 0; j < elements.length; j++) {
            const value = (checkedElement = elements.item(j)).value;

            if ((value == undefined) || (value === "")) {
                // Skip if empty is allowed or if editing
                if (checkV.allowEmpty || editing)
                    continue;
                else {
                    throwError(checkV.prefix + " may not be empty");
                    return false;
                }
            }

            // This is an unnecessary check for the login form
            if (!loggingIn && (checkV.minLength != undefined) && (value.length < checkV.minLength)) {
                throwError(checkV.prefix + "'s length may be greater than " + checkV.minLength + ".");
                return false
            }
            // This too is unnecessary since it's embedded to the fields anyways.
            // If someone messes with DevTools, they might as well just remove this check.
            if ((checkV.maxLength != undefined) && (value.length > checkV.maxLength)) {
                throwError(checkV.prefix + "'s length may not exceed " + checkV.maxLength + ".");
                return false
            }

            // HTML vulnerabilities 
            if (checkV.noBrackets && (name.includes("<") || name.includes(">"))) {
                throwError(checkV.prefix + " cannot include '<' or '>'");
                return false;
            }

            // Perform this field's custom check, if exist
            if ((checkV.check != undefined) && !checkV.check(value, loggingIn))
                return false;
        }

    }
    return true;
}