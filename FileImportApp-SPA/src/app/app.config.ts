import { Injectable } from '@angular/core';
import { IAppConfig } from './app-config.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppConfig {
    static settings: IAppConfig;

    constructor(private http: HttpClient) {}

    load() {
        const jsonFile = 'assets/config.json';
        return new Promise<void>((resolve, reject) => {
            this.http.get<IAppConfig>(jsonFile).subscribe((event) => {
                AppConfig.settings = event;
                resolve();
            }, (err) => {
                console.log('Cannot get configuration.');
            } );
        });
    }
}
