/**
* Selects an element specified by a selector.
* If Selector is a string, then it returns the first found element in document, if any, else null.
* If Selector is already an element, returns that element.
* Else returns null.
* @param {Element|string} Selector - An element or a string selector.
* @return {Element|null} Returns an element, if any, or null.
* @class
*/
var tp = function (Selector) {
    if (tp.IsString(Selector)) {
        return document.querySelector(Selector);
    } else if (tp.IsElement(Selector)) {
        return Selector;
    }

    return null;
};
 
//#region Errors

/**
 * Throws a tripous exception
 * @param {string} Message The exception message
 */
tp.Throw = function (Message) {
    let Ex = new Error(Message);
    Ex.name = 'tp-Error';
    throw Ex;
};
 

//#endregion
 
//#region Type checking

/** Type checking function. Returns true if the specified value is null or undefined.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsEmpty = function (v) { return v === null || v === void 0; };               // null or undefined
/** Type checking function. Returns true if the specified value is not null nor undefined.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsValid = function (v) { return !(v === null || v === void 0); };

/** Type checking function. Returns true if the specified value is an object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsObject = function (v) { return tp.IsValid(v) && typeof v === 'object'; };
/** Type checking function. Returns true if the specified value is an array.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsArray = function (v) { return Array.isArray(v); }; //  v instanceof Array || Object.prototype.toString.call(v) === '[object Array]';
/** Type checking function. Returns true if the specified value is a function.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsFunction = function (v) { return typeof v === 'function'; };
/** Type checking function. Returns true if the specified value is an arguments object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsArguments = function (v) { return Object.prototype.toString.call(v) === "[object Arguments]"; };

/** Type checking function. Returns true if the specified value is string.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsString = function (v) { return typeof v === 'string'; };
/** Type checking function. Returns true if the specified value is number.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsNumber = function (v) { return typeof v === 'number'; };
/** Type checking function. Returns true if the specified value is integer.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsInteger = function (v) { return typeof v === 'number' && v % 1 === 0; }
/** Type checking function. Returns true if the specified value is decimal number.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsFloat = function (v) { return typeof v === 'number' && n % 1 !== 0; }
/** Type checking function. . Returns true if the specified value is boolean.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsBoolean = function (v) { return typeof v === 'boolean'; };
/** Type checking function. . Returns true if the specified value is a date object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsDate = function (v) { return !tp.IsEmpty(v) && Object.prototype.toString.call(v) === "[object Date]"; };
/** Type checking function. Returns true if the specified value is string, number or boolean.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPrimitive = function (v) { return typeof v === 'string' || typeof v === 'number' || typeof v === 'boolean'; };
/** Type checking function. Returns true if the specified value is string, number, boolean or date.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsSimple = function (v) { return tp.IsPrimitive(v) || tp.IsDate(v); };

/** Type checking function. Returns true if the specified value is a promise object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPromise = function (v) { return v instanceof Promise; };
/** Type checking function. Returns true if the specified value is a RegExp object.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsRegExp = function (v) { return Object.prototype.toString.call(v) === "[object RegExp]"; };

/** Type checking function. Returns true if the specified value is an object but NOT a DOM element.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsPlainObject = function (v) { return tp.IsObject(v) && !v.nodeType; };
/** Type checking function. Returns true if the specified value is a DOM {@link Node}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsNode = function (v) { return v instanceof Node; };


/** Parses a json text and returns an object, array, string, number, boolean on success, else null.
 * @param {string} JsonText The json text to parse.
 * @returns {object} Returns an object, array, string, number, boolean on success, else null.
 */
tp.ParseJson = function (JsonText) {
    let Result = null;

    if (typeof JsonText === 'string' && !tp.IsBlank(JsonText)) {
        try {
            // JSON.parse returns an Object, Array, string, number, boolean, or null value corresponding to the given JSON text
            let o = JSON.parse(JsonText);
            Result = tp.IsValid(o) ? o : null;
        } catch (e) {
            //
        }
    }

    return Result;
};
/** Tries to parse a json text. <br />
 * Returns an object of type <code>{Value: object, Result: boolean}</code> where the Result is true on success and the Value is the parsed object.
 * @param {string} JsonText The json text to parse.
 * @returns {object} Returns an object of type <code>{Value: object, Result: boolean}</code> where the Result is true on success and the Value is the parsed object.
 */
tp.TryParseJson = function (JsonText) {
    let o = tp.ParseJson(JsonText);
    return {
        Value: o,
        Result: tp.IsValid(o)
    };
};
/**
 * Returns true if a specified string is a json string.
 * @param {string} Text The text to check
 * @returns {boolean} Returns true if a specified string is a json string.
 */
tp.IsJsonText = function (Text) {
    if (typeof Text === 'string' && !tp.IsBlank(Text)) {
        try {
            let o = JSON.parse(Text);
            return tp.IsValid(o);
        } catch (e) {
            //
        }
    }

    return false;
};


/** Type checking function for a DOM Node. Returns true if the specified value is a DOM attribute Node.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsAttribute = function (v) { return !!(v && v.nodeType === Node.ATTRIBUTE_NODE); };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM {@link Element}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsElement = function (v) { return v instanceof Element; };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM {@link HTMLElement}.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsHTMLElement = function (v) { return v instanceof HTMLElement; };
/** Type checking function for a DOM Node. Returns true if the specified value is a DOM text node.
 * @param {any} v The value to check
 * @returns {boolean} Returns true if the specified value passes the check.
 */
tp.IsText = function (v) { return !!(v && v.nodeType === Node.TEXT_NODE); };

/**
Type guard function for an HTMLElement that has a name attribute.
@param {Element} v - The element to check.
@returns {boolean} -
*/
tp.IsNamedHtmlElement = function (v) { return v instanceof HTMLElement && 'name' in v; };
/**
Type guard for an element that provides the querySelector() and querySelectorAll() methods, i.e. is document or Element
@param {Element} v - The element to check.
@returns {boolean} - Returns true if the specified element provides querySelector() and querySelectorAll() methods.
*/
tp.IsNodeSelector = function (v) { return tp.IsValid(v) && 'querySelector' in v && 'querySelectorAll' in v; };
/**
Type-guard function. An element providing checkValidity() and reportValidity() methods passes this test.
@see {@link https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/HTML5/Constraint_validation|Constraint validation}
@param {Element} v - The element to check.
@returns {boolean} - Returns true if the specified element provides checkValidity() and reportValidity() methods.
*/
tp.IsValidatableElement = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['checkValidity']) && tp.IsFunction(v['setCustomValidity']); };
/**
* Type-guard, mostly for the required property of form elements
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsFormElement = function (v) { return v instanceof HTMLInputElement || v instanceof HTMLSelectElement || v instanceof HTMLTextAreaElement; };

/**
Type guard function for the Cloneable interface
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsCloneable = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['Clone']); };
/**
Type guard function for the Assignable interface
* @param {Element} v - The element to check.
* @returns {boolean} -
*/
tp.IsAssignable = function (v) { return !tp.IsEmpty(v) && tp.IsFunction(v['Assign']); };

/**
True if a specified value is a DOM element of a certain type (e.g div, span, etc.)
@param {any} v Any value
@param {string} NodeName The node name  (e.g div, span, etc.)
@returns {boolean} Returns true if a specified value is a DOM element of a certain type (e.g div, span, etc.)
*/
tp.ElementIs = function (v, NodeName) { return tp.IsElement(v) && tp.IsSameText(v.nodeName, NodeName); };

//#endregion
 
//#region Strings



/**
 Returns true if a specified string is null, undefined or it just contains white space chars (space, tab, etc). <br />
 Throws an exception if the specified value is other than undefined, null or string.
@param {string} v - A string value. 
@returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars (space, tab, etc)
*/
tp.IsBlank = function (v) {
    if (v === void 0 || v === null)
        return true;

    if (!tp.IsString(v)) {
        tp.Throw('Can not check for null or whitespace a non-string value');
    }

    return v.trim().length === 0; //v.replace(/\s/g, '').length < 1;
};
/**
 Returns true if a specified string is null, undefined or it just contains white space chars
@param {string} v - A string value.
@returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars
*/
tp.IsNullOrWhiteSpace = function (v) { return tp.IsBlank(v); };
/**
 * Returns true if a specified string is null, undefined or it just contains white space chars (space, tab, etc). <br />
 * No exception is thrown if the specified value is other than undefined, null or string.
 * @param {string} v - A string value.
 * @returns {boolean}  Returns true if the string is null, undefined or it just contains white space chars (space, tab, etc)
 */
tp.IsBlankString = function (v) {
    return (v === void 0 || v === null) || (tp.IsString(v) && v.trim().length === 0);
};

/** True if a specified character is a white space char (space, tab, etc)  
@param {character} c - A character value. 
@returns {boolean} Returns True if a specified character is a white space char (space, tab, etc)
*/
tp.IsWhitespaceChar = function (c) { return c.charCodeAt(0) <= 32; }; // return ' \t\n\r\v'.indexOf(c) === 0;
/**
Returns true if a specified text looks like html markup.
@see {@link https://stackoverflow.com/questions/15458876/check-if-a-string-is-html-or-not|stackoverflow}
@param {string} Text - The text to test.
@returns {boolean} Returns true if a specified text looks like html markup.
*/
tp.IsHtml = function (Text) { return /<[a-z][\s\S]*>/i.test(Text); };


/**
 True if two string are of the same text, case-insensitively always
@param {string} A - The first string.
@param {string} B - The second string
@returns {boolean} Returns true if the two strings are case-insensitively identicals.
*/
tp.IsSameText = function (A, B) {
    return tp.IsString(A) && tp.IsString(B) && A.toUpperCase() === B.toUpperCase();
};
/**
 True if a  sub-string is contained by another string
@param {string} Text - The string
@param {string} SubText - The sub-string, the string to search for.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
@returns {boolean} Returns true if a substring is contained in the other string.
*/
tp.ContainsText = function (Text, SubText, CI = true) {
    CI = CI === true;
    if (tp.IsString(Text) && !tp.IsBlank(Text)) {
        return CI ? Text.toLowerCase().includes(SubText.toLowerCase()) : Text.includes(SubText);
    }

    return false;
};
/**
Inserts a sub-string in another string at a specified index and returns the new string.
@param {string} SubString - The sub-string to insert.
@param {string} Text - The string
@param {number} Index - The position at the string where the sub-string should be inserted.
@returns {string} Returns the new string.
*/
tp.InsertText = function (SubString, Text, Index) {
    return [Text.slice(0, Index), SubString, Text.slice(Index)].join('');
};

 

/**
Trims a string (removes blank characters from start and end) and returns the new string.
@param {string} v - The string .
@returns {string} Returns the new string.
*/
tp.Trim = function (v) {
    return tp.IsBlank(v) ? "" : v.trim(); //v.replace(/^\s+|\s+$/g, "");
};
/**
Trims a string by removing blank characters from the start of the string and returns the new string.
@param {String} v - The string .
@returns {string} Returns the new string.
*/
tp.TrimStart = function (v) {
    return tp.IsBlank(v) ? "" : v.trimStart(); //v.replace(/^\s+/, "");
};
/**
Trims a string by removing blank characters from the end of the string and returns the new string.
@param {string} v - The string .
@returns {string}  Returns the new string.
*/
tp.TrimEnd = function (v) {
    return tp.IsBlank(v) ? "" : v.trimEnd(); //v.replace(/\s+$/, "");
};

/**
  True if a string starts with a sub-string.
 @param {string} Text - The string to check.
 @param {string} SubText - The sub-string, the string to search for.
 @param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
 @returns {boolean} Returns true if a string starts with a sub-string.
 */
tp.StartsWith = function (Text, SubText, CI = true) {
    if (tp.IsBlank(SubText) || tp.IsBlank(Text))
        return false;

    if (tp.IsEmpty(CI)) {
        CI = true;
    }

    let S = Text.substring(0, SubText.length);

    return CI === true ? S.toUpperCase() === SubText.toUpperCase() : S === SubText;

};
/**
 True if a string ends with a sub-string.
@param {string} Text - The string
@param {string} SubText - The sub-string, the string to search for.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false
@returns {boolean} Returns true if a string ends with a sub-string.
*/
tp.EndsWith = function (Text, SubText, CI = true) {
    if (tp.IsBlank(SubText) || tp.IsBlank(Text))
        return false;

    if (tp.IsEmpty(CI)) {
        CI = true;
    }

    let S = Text.substring(Text.length - SubText.length, Text.length);

    return CI === true ? S.toUpperCase() === SubText.toUpperCase() : S === SubText;
};
/**
Replaces a sub-string by another sub-string, inside a string, and returns the new string.
@param {string} v - The string .
@param {string} OldValue - The old string .
@param {string} NewValue - The new string .
@returns {string} Returns the new string.
*/
tp.Replace = function (v, OldValue, NewValue) {
    return v.replace(OldValue, NewValue);
};
/**
 Replaces all occurences of a sub-string by another sub-string, inside a string, and returns the new string.
@param {string} v - The string .
@param {string} OldValue - The old string.
@param {string} NewValue - The new string.
@param {boolean} [CI=true] - CI (Case-Insensitive) can be true (the default) or false 
@returns {string} Returns the string after the replacement.
*/
tp.ReplaceAll = function (v, OldValue, NewValue, CI = true) {
    OldValue = tp.RegExEscape(OldValue);
    var Flags = CI === true ? 'igm' : 'gm';
    return v.replace(new RegExp(OldValue, Flags), NewValue);
};
/**
Replaces a character found at a specified index inside a string, and returns the new string.
@param {string} v - The string .
@param {character} NewChar - The character that replaces the old character.
@param {number} Index - The index of the character to be replaced
@returns {string} Returns the string after the replacement.
*/
tp.ReplaceCharAt = function (v, NewChar, Index) {
    return [v.slice(0, Index), NewChar, v.slice(Index + 1)].join('');
};

/**
Places single or double quotes around a string (defaults to single quotes), and returns the new string.
@param {string} v - The string .
@param {boolean} [DoubleQuotes=true] When true then double quotes are used.
@returns {string} Returns the result string
*/
tp.Quote = function (v, DoubleQuotes = true) {
    DoubleQuotes = DoubleQuotes === true;

    if (tp.IsValid(v)) {
        if (DoubleQuotes) {
            v = v.replace(/"/gm, '\\"');
            v = '"' + v + '"';
        } else {
            v = v.replace(/'/gm, "\\'");
            v = "'" + v + "'";
        }
    }

    return v;
};
/**
Unquotes a string if it is surrounded by single or double quotes, and returns the new string.
@param {string} v - The string .
@returns {string} Returns the result string
*/
tp.Unquote = function (v) {
    if (tp.IsValid(v)) {
        if (v.charAt(0) === '"') {
            return v.replace(/(^")|("$)/g, '');
        } else if (v.charAt(0) === "'") {
            return v.replace(/(^')|('$)/g, '');
        }
    }

    return v;
};
/**
Trims a specified string and if the last character is the comma character it removes it. Returns the new string.
@param {string} v - The string
@returns {string} Returns the new string.
*/
tp.RemoveLastComma = function (v) {
    if (tp.IsBlank(v))
        return '';
    else {
        v = tp.Trim(v);
        if (v.length > 0 && tp.EndsWith(v, ','))
            v = v.substring(0, v.length - 1);

        return v;
    }
};
/**
Splits a string into chunks according to a specified chunk size and returns an array of strings.
@param {string} v - The string .
@param {number} ChunkSize - The size in characters for each chunk.
@returns {string[]} Returns Array of strings.
*/
tp.Chunk = function (v, ChunkSize) {
    var rg = new RegExp('.{1,' + ChunkSize + '}', 'g');
    var A = v.match(rg);
    return A;
};
/**
Splits a string into an array of strings by separating the string into and an array of substrings.  
The separator is treated as a string or a regular expression. 
If separator is omitted or does not occur in string, the array returned contains one element consisting of the entire string.  
If separator is an empty string, then the string is converted to an array of characters.
@param   {string}  v - The string to operate on
@param   {string} [Separator=' '] - Optional. Specifies the character(s) to use for separating the string.
@param   {boolean} [RemoveEmptyEntries=true] Optional. When true, the default, then empty entries are removed from result.
@returns  {string[]}  Returns an array of strings.
*/
tp.Split = function (v, Separator = ' ', RemoveEmptyEntries = true) {
    v = v || '';
    RemoveEmptyEntries = RemoveEmptyEntries === true;

    if (RemoveEmptyEntries) {
        var Parts = v.split(Separator);
        var A = [];
        for (var i = 0, ln = Parts.length; i < ln; i++) {
            if (!tp.IsBlank(Parts[i]))
                A.push(Parts[i]);
        }
        return A;
    } else {
        return v.split(Separator);
    }

};
/**
 * Splits a string like "ThisIsAString" into a string like "This Is A String".
 * @param {string} v The string to split.
 */
tp.SplitOnUpperCase = function (v) {
    let Result = '';
    if (tp.IsString(v)) {
        Result = v.match(/[A-Z][a-z]+/g).join(' ');
    }
    return Result;
};
/**
Splits a string of a certain format and creates and returns a javascript object.  
The input string MUST have the following format:
  Key0:Value0; Key1:Value1; KeyN:ValueN; 
Key is a string (with single or double quotes, or no quotes at all).
Value could be any value, or string (with single or double quotes, or no quotes at all).
@param  {string} v - The string to operate on
@returns {object} Returns the constructed object.
 */
tp.SplitDescriptor = function (v) {

    var Result = {};

    if (tp.IsString(v)) {
        var Lines = tp.Split(v, ";", true);
        var parts;

        var Key;
        var Value;

        if (Lines) {
            for (var i = 0; i < Lines.length; i++) {
                parts = tp.Split(Lines[i], ":");
                if (parts && parts.length === 2) {
                    Key = tp.Unquote(Trim(parts[0]));
                    Value = tp.Unquote(Trim(parts[1]));

                    if (Key.length && Value.length) {
                        Result[Key] = Value;
                    }
                }
            }
        }
    }

    return Result;
};
/**
Returns a united string by joining a list of values using an optional separator.  
@param {string} Separator - The separator to use.
@param  {...any} values - The values to join
@returns {string} Returns the result string.
 */
tp.Join = function (Separator, ...values) {

    Separator = Separator || '';

    var i, ln, Params = [];
    for (i = 1, ln = arguments.length; i < ln; i++) {
        Params.push(arguments[i]);
    }

    return Params.join(Separator);
};
/**
Returns a united string by joining a list of values using as separator a comma and a space. 
@param {...any} values The values to join
@returns {string} Returns the result string.
 */
tp.CommaText = function (...values) {

    var i, ln, Params = [];
    for (i = 0, ln = arguments.length; i < ln; i++) {
        Params.push(arguments[i]);
    }
    return Params.join(',  ');
};
/**
Returns an array of strings, by splitting a string, considering the line breaks (\n or \r\n) as separator 
@param  {string} v The string to operate on.
@returns {string[]} Returns an array of strings
 */
tp.ToLines = function (v) {
    if (!tp.IsBlank(v)) {
        var SEP = '##__##';
        v = tp.ReplaceAll(v, '\r\n', SEP);
        v = tp.ReplaceAll(v, '\n', SEP);
        return v.split(SEP);
    }
    return [];
};
/**
Returns a html string, by replacing line breaks (\n or \r\n) with <code>&lt;br /&gt;</code> elements. 
@param  {string} v The string to operate on.
@returns {string} Returns the new string.
 */
tp.LineBreaksToHtml = function (v) {
    if (!tp.IsBlank(v)) {
        var SEP = '##__##';
        v = tp.ReplaceAll(v, '\r\n', SEP);
        v = tp.ReplaceAll(v, '\n', SEP);
        v = tp.ReplaceAll(v, SEP, '');
    }

    return v;
};
/** Replaces line breaks (\r\n, \r and \n) with a specifed separator string and returns the new string.
 * @param {string} v The string to operate on
 * @param {string} sep The separator that replaces the line breaks.
 * @returns {string} Replaces line breaks (\r\n, \r and \n) with a specifed separator string and returns the new string.
 */
tp.ReplaceLineBreaks = function (v, sep) {
    if (!tp.IsBlank(v)) {
        v = tp.ReplaceAll(v, '\r\n', sep);
        v = tp.ReplaceAll(v, '\r', sep);
        v = tp.ReplaceAll(v, '\n', sep);
    }
    return v;
};
/**
Creates and returns a string by repeating a string value a certain number of times
@param  {string} v - The string to repeat.
@param {number} Count - How many times to repeat the input string
@returns {string} Returns a string by repeating a string value a certain number of times
 */
tp.Repeat = function (v, Count) {
    var Result = "";

    for (var i = 0; i < Count; i++) {
        Result += v;
    }
    return Result;
};
/**
Pads a string from left side (start) with a specified sub-string until a specified total length
@param  {string} v - The string to operate on.
@param  {string} PadText - The string to be used in padding.
@param {number} TotalLength - The desired total length of the result string
@returns {string} Returns the padded string.  
 */
tp.PadLeft = function (v, PadText, TotalLength) {
    if (!tp.IsValid(v))
        return v;

    TotalLength = ~~TotalLength;
    v = String(v);
    if (v.length < TotalLength) {
        PadText = tp.Repeat(PadText, TotalLength - v.length);
        v = PadText + v;
    }

    return v;
};
/**
Pads a string from right side (end) with a specified sub-string until a specified total length
@param  {string} v - The string to operate on.
@param  {string} PadText - The string to be used in padding.
@param {number} TotalLength - The desired total length of the result string
@returns {string} Returns the padded string.
 */
tp.PadRight = function (v, PadText, TotalLength) {
    if (!tp.IsValid(v))
        return v;

    TotalLength = ~~TotalLength;
    v = String(v);
    if (v.length < TotalLength) {
        PadText = tp.Repeat(PadText, TotalLength - v.length);
        v = v + PadText;
    }

    return v;
};
/**
Truncates a string to a specified length if string's length is greater than the specified length. Returns the truncated string.
@param  {string} v - The string to operate on.
@param {number} NewLength - The length of the result string
@returns {string} Returns the new string.
 */
tp.SetLength = function SetLength(v, NewLength) {
    if (tp.IsBlank(v))
        return "";

    v = String(v);
    NewLength = ~~NewLength;
    if (v.length > NewLength) {
        v = v.slice(0, NewLength);
    }
    return v;
};

/** Returns true if a specified string is a valid identifier name
 * @param {string} v The string to check.
 * @param {string} [PlusValidChars=''] Optional. User defined valid characters, other than the first character, e.g. '$'.
 * @returns {boolean} Returns true if a specified string is a valid table name
 */
tp.IsValidIdentifier = function (v, PlusValidChars = '') {
    if (!tp.IsString(v) || tp.IsBlank(v))
        return false;

    PlusValidChars = tp.IsString(PlusValidChars) ? PlusValidChars : '';

    let SLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    let SNumbers = "0123456789";
    let SStartLetters = SLetters + '_';
    let SValidChars = SLetters + SNumbers + PlusValidChars + '_';

    for (let i = 0, ln = v.length; i < ln; i++) {
        c = v.charAt(i);
        if ((i === 0) && !SStartLetters.includes(c))
            return false;

        if (!SValidChars.includes(c))
            return false;
    }

    return true;
}

/** Used as the return value by number convertion functions 
 */
tp.NumberConvertionResult = class {

    /**
     * Constructor
     * @param {number} Value The value after the convertion
     * @param {boolean} Result Result of the convertion 
     */
    constructor(Value = 0, Result = false) {
        this.Value = Value || 0;
        this.Result = Result === true;
    }

    /** The value after the convertion
     @type {number}
     */
    Value = 0;
    /** Result of the convertion
     @type {boolean}
     */
    Result = false;

};

/**
Tries to convert a string into an integer.   <br />
Returns a {@link tp.NumberConvertionResult} object as <code>{Value: v, Result: true}</code> where Value is the convertion result, if successful, and Result indicates success or failure.
@param  {string} v - The string to operate on.
@returns {tp.NumberConvertionResult} Returns an {@link tp.NumberConvertionResult} object as <code> {Value: v, Result: true}</code>
 */
tp.TryStrToInt = function (v) {
    let NCR = new tp.NumberConvertionResult();

    if (tp.IsNumber(v)) {
        v = tp.Truncate(v);
        NCR.Value = v;
        NCR.Result = true;
    }
    else if (!tp.IsBlankString(v)) {
        try {
            v = parseInt(v, 10);
            NCR.Value = v;
            NCR.Result = isNaN(v) ? false : true;
        } catch (e) {
            //
        }
    }

    return NCR;
};
/**
Tries to convert a string into a float.  <br />
NOTE: The decimal separator could be point or comma. <br />
Returns a {@link tp.NumberConvertionResult} object as <code>{Value: v, Result: true}</code>  where Value is the convertion result, if successful, and Result indicates success or failure.
@param  {string} v - The string to operate on.
@returns {tp.NumberConvertionResult} Returns a {@link tp.NumberConvertionResult} object as <code> {Value: v, Result: true}</code>
 */
tp.TryStrToFloat = function (v) {

    let NCR = new tp.NumberConvertionResult();

    if (tp.IsNumber(v)) {
        NCR.Value = v;
        NCR.Result = true;
    }
    else if (!tp.IsBlankString(v)) {
        try {
            v = v.replace(',', '.');
            v = parseFloat(v, 10);
            NCR.Value = v;
            NCR.Result = isNaN(v) ? false : true;
        } catch (e) {
            //
        }
    }

    return NCR;

};

/**
Converts a string (or a number) to an integer and returns that integer. <br />
Uses the "try" version to perform the convertion.
@param  {string} v - The string to operate on.
@param {number} [Default=0] - The default value to return if the convertion is not possible. Defaults to 0.
@returns {number} Returns the number.
 */
tp.StrToInt = function (v, Default = 0) {
    let NCR = tp.TryStrToInt(v);
    return NCR.Result === true ? NCR.Value : Default;
};
/**
Converts a string (or a number) to a float and returns that float. <br />
Uses the "try" version to perform the convertion.
@param  {string} v - The string to operate on.
@param {number} [Default=0] - The default value to return if the convertion is not possible. Defaults to 0.
@returns {number} Returns the number.
 */
tp.StrToFloat = function (v, Default = 0) {
    let NCR = tp.TryStrToFloat(v);
    return NCR.Result === true ? NCR.Value : Default;
};
/**
Converts a string to a boolean and returns that boolean.  
The input string must be either 'true' or 'false' regardless of case-sensitivity. 
@param  {string} v - The string to operate on.
@param {boolean} [Default=false] - The default value to return if the convertion is not possible. Defaults to false.
@returns {boolean} Returns the boolean value.
 */
tp.StrToBool = function (v, Default = false) {
    Default = Default === true;

    if (tp.IsSameText(v, "true") || tp.IsSameText(v, "yes")) {
        return true;
    } else if (tp.IsSameText(v, "false") || tp.IsSameText(v, "no")) {
        return false;
    }

    return Default;
};


/**
Converts an integer value into a hexadecimal string, and returns that string   
@param  {number} v - The value to operate on.
@returns {string} Returns the hex string.
 */
tp.ToHex = function (v) {
    if (v < 0) {
        v = 0xFFFFFFFF + v + 1; // ensure not a negative number
    }

    var S = v.toString(16).toUpperCase();
    while (S.length % 2 !== 0) {
        S = '0' + S;
    }
    return S;
};
/**
Escapes a string for use in Javascript regex and returns the escaped string   
@param  {string} v - The value to operate on.
@returns {string} Returns the escaped string.
@see {@link https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex|StackOverflow}
 */
tp.RegExEscape = function (v) {
    //return v.replace(/([.*+?\^=!:${}()|\[\]\/\\])/g, "\\$1");
    return tp.IsBlank(v) ? "" : v.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
};
/**
 Converts a dashed string to camel case, e.g. background-color to backgroundColor and -webkit-user-select to webkitUserSelect and returns the new string  
@param  {string} v - The value to operate on.
@returns {string} Returns the the camel-cased string.
 */
tp.DashToCamelCase = function (v) {
    if (!tp.IsBlank(v)) {
        if (v.length > 1 && v.charAt(0) === '-') {
            v = v.substring(1);
        }

        v = v.replace(/-([\da-z])/gi, function (match, c) {
            return c.toUpperCase();
        });
    }

    return v;
};
/**
 Combines two strings by returning a single url path. Ensures that the required slashes are in place.
@param {string} A The first string.
@param {string} B The second string.
@returns {string} The combination of the two strings.
*/
tp.UrlCombine = function (A, B) {
    if (!tp.EndsWith(A, '/') && !tp.StartsWith(B, '/'))
        A += '/';
    return A + B;
};
/**
 Combines a TableName a dot and a FieldName and returns a string. If no TableName is specified then just the FieldName is returned.
@param {string} TableName Optional. The table name
@param {string} FieldName The field name
@returns {string} Returns the combined string, e.g. Customer.Name or just Name
*/
tp.FieldPath = function (TableName, FieldName) {
    if (!tp.IsBlankString(FieldName)) {
        return !tp.IsBlankString(TableName) ? TableName + '.' + FieldName : FieldName;
    }

    return '';
};
/**
 Combines a TableName a double dash (__) and a FieldName and returns a string. If no TableName is specified then just the FieldName is returned.
@param {string} TableName The table name
@param {string} FieldName The field name
@returns {string} Returns the combined string, e.g. Customer__Name
*/
tp.FieldAlias = function (TableName, FieldName) {
    if (!tp.IsBlankString(FieldName)) {
        return !tp.IsBlankString(TableName) ? TableName + '__' + FieldName : FieldName;
    }

    return '';
};
/**
Returns a new GUID string.
@param {boolean} [UseBrackets=false] Optional. If true, then ther result string is encloded by brackets. Defaults to false.
@returns {string} Returns a GUID string.
@see {@link https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript|StackOverflow}
*/
tp.Guid = function (UseBrackets = false) {

    var d = new Date().getTime();
    if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
        d += performance.now(); //use high-precision timer if available
    }

    var Result = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16).toUpperCase();
    });

    UseBrackets = UseBrackets || false;
    return !UseBrackets ? Result : "{" + Result + "}";
};
/** Creates and returns a random string of a specified length, picking characters from a specified set of characters.
 * @param {number} Length The length or the string to create
 * @param {string} CharSet The set of characters to pick from.
 * @returns {string} Returns a random string of a specified length, picking characters from a specified set of characters.
 */
tp.GenerateRandomString = function (Length, CharSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789') {
    if (tp.IsBlank(CharSet))
        CharSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';

    let Buffer = [];
    let Index, c;

    for (let i = 0, ln = Length; i < ln; i++) {
        Index = tp.Random(0, CharSet.length - 1);
        c = CharSet.charAt(Index);
        Buffer.push(c);
    }

    let Result = Buffer.join('');
    return Result;


};
/**
Creates and returns a function from a string
@param {string} v - The function source code
@example
var func = tp.CreateFunction('function (a, b) { return a + b; }');
@returns {function} Returns a function  
@see {@link http://stackoverflow.com/questions/7650071/is-there-a-way-to-create-a-function-from-a-string-with-javascript|StackOverflow}
*/
tp.CreateFunction = function (v) {
    var funcReg = /function *\(([^()]*)\)[ \n\t]*\{(.*)\}/gmi;
    var match = funcReg.exec(v.replace(/\n/g, ' '));

    if (match) {
        var Args = match[1].split(',');
        Args.push(match[2]);
        return Function.apply(null, Args);
    }

    return null;
};

/**
Creates a base-64 encoded ASCII string from a string value.
@param {string} v - The value to operate on
@returns {string} Returns a base-64 string.
@see {@link https://stackoverflow.com/questions/30106476/using-javascripts-atob-to-decode-base64-doesnt-properly-decode-utf-8-strings|StackOverflow}
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding|MDN}
*/
tp.ToBase64 = function (v) {
    return window.btoa(encodeURIComponent(v).replace(/%([0-9A-F]{2})/g, function (match, p1) {
        return String.fromCharCode(Number('0x' + p1.toString()));
    }));
};
/**
Decodes a string of data which has been encoded using base-64 encoding.
@param {string} v - The value to operate on
@returns {string} Returns the plain string.
@see {@link https://stackoverflow.com/questions/30106476/using-javascripts-atob-to-decode-base64-doesnt-properly-decode-utf-8-strings|StackOverflow}
@see {@link https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding|MDN}
*/
tp.FromBase64 = function (v) {
    return window.atob(v);
};

//#endregion

//#region Url handling
/**
 * Navigates to a specified url
 * @param {string} Url Url to navigate to
 */
tp.NavigateTo = function (Url) {
    if (!tp.IsBlank(Url))
        window.location.href = Url;
};
/**
Returns the base url, e.g http://server.com/
@returns {string} Returns the base url
*/
tp.GetBaseUrl = function () { return window.location.protocol + "//" + window.location.host + "/"; };
/**
 * Returns a query string parameter by name, if any, else null
 * @param {string} Name - The name of the parameter
 * @param {string} [Url] - Optional. If not specified then the current url is used
   @returns {string} Returns a query string parameter by name, if any, else null
 */
tp.ParamByName = function (Name, Url = null) {
    if (!Url)
        Url = window.location.href;

    Name = Name.replace(/[\[\]]/g, "\\$&");

    var regex = new RegExp("[?&]" + Name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(Url);

    if (!results)
        return null;

    if (!results[2])
        return '';

    return decodeURIComponent(results[2].replace(/\+/g, " "));
};

/**
 * Returns a plain object where each property is a query string parameter.
 * @param {string} [Url] - Optional. If not specified then the current url is used
   @returns {object} Returns a plain object where each property is a query string parameter.
 */
tp.GetParams = function (Url = null) {
    if (!Url)
        Url = window.location.href;

    var Result = {};

    var Index = Url.indexOf('?');

    if (Index !== -1) {
        var Parts,
            List = Url.slice(Index + 1).split('&');

        for (var i = 0; i < List.length; i++) {
            Parts = List[i].split('=');

            Result[Parts[0]] = decodeURIComponent(Parts[1]);
        }
    }

    return Result;
};

//#endregion
















function tp_ShowMessage() {
    alert('Hi tp javascript');
}