import moment from 'moment';

export const dateFormat = 'DD.MM.YYYY.';
export const dateTimeFormat = 'DD.MM.YYYY. HH:mm:ss';
export const dateFormatForApi = 'YYYY-MM-DD';

export function getToday(): Date {
    return new Date();
}

export function getDateString(dateTime: Date, format: string): string {
    return moment(dateTime).format(format);
}

export function getDateForAPI(dateTime: Date): string {
    return moment(dateTime).format(dateFormatForApi);
}

export const convertBase64ToBlobData = (base64Data: string, contentType: string, sliceSize = 512) => {
    const byteCharacters = atob(base64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        const slice = byteCharacters.slice(offset, offset + sliceSize);

        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);

        byteArrays.push(byteArray);
    }

    return new Blob(byteArrays, { type: contentType });
};
export const download = (base64content: string, filename: string, contentType: string): void => {
    const blobData = convertBase64ToBlobData(base64content, contentType);

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        //IE
        window.navigator.msSaveOrOpenBlob(blobData, filename);
    } else {
        // chrome, firefox ...
        const blob = new Blob([blobData], { type: contentType });
        const url = window.URL.createObjectURL(blob);
        // window.open(url);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();
    }
};
