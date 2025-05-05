
//#region tp.AjaxContentType

/** Ajax content types
 @class 
 */
tp.AjaxContentType = {
    /** Json content
     * @type {string}
     */
    Json: 'application/json; charset=UTF-8',
    /** This is the default format used by HTML forms.
     * In this format, the data is encoded as key-value pairs separated by &amp; characters, with the key and value separated by = characters. 
     * For example, key1=value1&amp;key2=value2 .
     * This format is simple and efficient, but it has limitations in terms of the types of data that can be sent.
     * @type {string}
     */
    FormUrlEncoded: 'application/x-www-form-urlencoded; charset=UTF-8',
    /** This is a more flexible format, used by HTML forms, and can be used to send binary data, such as files, as well as text data.
     * In this format, the data is divided into multiple parts, each with its own set of headers.
     * Each part is separated by a boundary string, which is specified in the Content-Type header.
     * This format is more complex than application/x-www-form-urlencoded, but it allows for more types of data to be sent.
     * @type {string}
     */
    MultipartFormData: 'multipart/form-data; charset=UTF-8'
};
Object.freeze(tp.AjaxContentType);

//#endregion  

//#region tp.AjaxArgs

/**
Arguments class for the {@link tp.Ajax} class methods.
*/
tp.AjaxArgs = class {

    /**
     * Constructor.
     * Creates an arguments object for use with the {@link tp.Ajax} class methods
     * @param {object|tp.AjaxArgs} SourceArgs - Optional. A source arguments object to copy property values from. Could be a {@link tp.AjaxArgs} instance.
     */
    constructor(SourceArgs = null) {

        // default initialization
        this.Method = "POST";
        this.Url = '';

        this.Data = null;                       // the data to send
        this.UriEncodeData = true;
        this.Timeout = 0;
        this.ContentType = 'application/x-www-form-urlencoded; charset=UTF-8';
        this.Context = null;                                                    // context for calling the two callbacks
        this.AntiForgeryToken = '';                                             // used when POST-ing an html form in Microsoft MVC framework
        this.OnSuccess = null;                                                  // function(Args: tp.AjaxArgs)
        this.OnFailure = null;                                                  // function(Args: tp.AjaxArgs)
        this.OnRequestHeaders = tp.AjaxOnRequestHeadersDefaultHandler;          // function(Args: tp.AjaxArgs)
        this.ResponseHandlerFunc = tp.AjaxResponseDefaultHandler;

        this.XHR = null;                        // XMLHttpRequest
        this.ErrorText = '';                    // the XMLHttpRequest.statusText in case of an error
        this.Result = false;                    // true if ajax call succeeded

        this.ResponseData = {                   // server response  
            IsSuccess: false,
            ErrorText: '',
            Packet: {}
        };

        this.Tag = null;                        // a user defined value

        // apply the specified parameteter
        SourceArgs = SourceArgs || {};

        for (var Prop in SourceArgs) {
            this[Prop] = SourceArgs[Prop];
        }
    }

    /* properties */
    /** Returns true if a POST method is specified. 
     @type {boolean}
     */
    get IsPost() {
        return tp.IsSameText('post', this.Method);
    }
    /** Returns true if a GET method is specified.
    @type {boolean} */
    get IsGet() {
        return !this.IsPost;
    }

    /** The XMLHttpRequest.responseText string in any case (could be null though in case of an error). <br />
     * <strong>Valid only after response from server</strong>
     @type {string}
     */
    get ResponseText() {
        return this.XHR ? this.XHR.responseText : '';
    }


    /** Returns a string representation of this instance 
     @returns {string} Returns a string representation of this instance
     */
    toString() {
        let S = `
Method: "${this.Method}"
Url: "${this.Url}"
AjaxResult: "${this.Result}"
ErrorText: "${this.ErrorText}"
ResponseText: "${this.ResponseText}"
ResponseResult: "${this.ResponseData.IsSuccess}"
ResponseErrorText: "${this.ResponseData.ErrorText}" `;

        return S;
    }
};
/** The Http method to use. Defaults to POST. 
@default POST
@type {string} */
tp.AjaxArgs.prototype.Method = "POST";
/** The url.  
 @type {string} */
tp.AjaxArgs.prototype.Url = '';
/** Represents the data to send. Defaults to null. <br />
 * In POSTs it is a plain object with one or more key/value pairs. <br />
 * In GETs can be null or empty, or a string with query parameters, e.g. <code>param1=value1&paramN=valueN</code>
 * @default null
 * @type {object} */
tp.AjaxArgs.prototype.Data = null;
/** When true, then Data is Uri-encoded. Defaults to true.
 * @default true
 * @see {@link http://stackoverflow.com/questions/18381770/does-ajax-post-data-need-to-be-uri-encoded|stackoverflow}
*/
tp.AjaxArgs.prototype.UriEncodeData = true;
/** The timeout in milliseconds. <br />
 * Defaults to zero, meaning no timeout. <br />
 * When set to a non-zero value will cause fetching to terminate after the given time has passed. 
 @default 0
 @type {number}
 */
tp.AjaxArgs.prototype.Timeout = 0;
/** The content type. Defaults to <code>application/x-www-form-urlencoded; charset=UTF-8</code>
 @default application/x-www-form-urlencoded; charset=UTF-8
 @type {string}
 */
tp.AjaxArgs.prototype.ContentType = 'application/x-www-form-urlencoded; charset=UTF-8';
/** context for calling the two callbacks  
 @default null
 @type {object}
 */
tp.AjaxArgs.prototype.Context = null;
/** A string used when POST-ing an html form in Microsoft MVC framework. Defaults to empty string.  
 @default ''
 */
tp.AjaxArgs.prototype.AntiForgeryToken = '';
/** A <code>function(Args: tp.AjaxArgs)</code> callback function to call on success  
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnSuccess = null;
/** A <code>function(Args: tp.AjaxArgs)</code> callback function to call on failure
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnFailure = null;
/** A function(Args: tp.AjaxArgs) callback function to call in order to give the caller code a chance to add additional request headers.
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.OnRequestHeaders = null;
/**
 * A function(Args: tp.AjaxArgs) callback function. It is called just before the OnSuccess() call-back. <br />
 * Processes the response after an ajax call returns. <br />
 * The default response handler deserializes the Args.ResponseText into an object and assigns the Args.ResponseData object.
 * It assumes that the ResponseText is a json text containing an object with at least two properties: <code> { IsSuccess: boolean, ErrorText: string } </code>. <br />
 * Further on, if the Args.ResponseData contains a Packet property and that Packet property is a json text, deserializes it into an object.
 @default null
 @type {function}
 */
tp.AjaxArgs.prototype.ResponseHandlerFunc = null;

/** The {@link https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest|XMLHttpRequest} object. <br />
 * <strong>Valid only after response from server</strong>
 @type {XMLHttpRequest}
 */
tp.AjaxArgs.prototype.XHR = null;
/** The XMLHttpRequest.statusText string in case of an error. <br />
 * <strong>Valid only after response from server</strong>
 @type {string}
 */
tp.AjaxArgs.prototype.ErrorText = '';
/** True when ajax call succeeds 
 @type {boolean}
 */
tp.AjaxArgs.prototype.Result = false;
/** The response from the server it is always packaged as a C# Tripous.HttpActionResult instance. <br />
 * That is it comes as an object <code>{ IsSuccess: false, ErrorText: '', Packet: {} }</code>. <br />
 * The default response handler places the parsed json object in the Packet property of the ResponseData on success. <br />
 * <strong>Valid only after response from server</strong>
 @type {object}
 */
tp.AjaxArgs.prototype.ResponseData = { IsSuccess: false, ErrorText: '', Packet: {} };
/** The default response handler places the parsed json object in the Packet on success.
 * @type {object}
 * */
tp.AjaxArgs.prototype.Packet = null;
/** A user defined value.  
 @default null
 @type {any}
 */
tp.AjaxArgs.prototype.Tag = null;

//#endregion