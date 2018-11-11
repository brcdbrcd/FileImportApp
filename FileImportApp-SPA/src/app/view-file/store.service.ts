import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StoreResponse } from './StoreResponse';
import { AppConfig } from './../app.config';

@Injectable()
export class StoreService {
    constructor(private http: HttpClient) { }

    getData(session: String, pageIndex: number ,pageSize: number = 10) {
        const backendURL = AppConfig.settings.apiServer.url;
        return this.http.get<StoreResponse>(`${backendURL}/api/file/data?session=${session}&pageNum=${pageIndex}&pageSize=${pageSize}`); 
    }


}
