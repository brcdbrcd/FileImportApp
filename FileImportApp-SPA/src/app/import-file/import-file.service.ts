import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { ImportState } from './ImportState';
import { ImportResponse } from './ImportResponse';
import { AppConfig } from './../app.config';


@Injectable()
export class ImportService {
    backendURL: String;

    constructor(private http: HttpClient) {
        this.backendURL = AppConfig.settings.apiServer.url;
    }

    uploadData(data) {
        const formData = new FormData();
        formData.append('file', data);
        const req = new HttpRequest('POST', `${this.backendURL}/api/file/import`, formData, {reportProgress: true});
        return this.http.request<ImportResponse>(req);
    }

    /**
     * Returns percentage
     */
    getStatus(session: String) {
        return this.http.get<ImportState>(`${this.backendURL}/api/file/import/status?session=${session}`);
    }

}
