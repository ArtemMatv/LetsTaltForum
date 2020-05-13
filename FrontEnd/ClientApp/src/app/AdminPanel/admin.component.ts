import { Component, OnInit } from '@angular/core';
import { NavbarFooterService } from '../_services/nav-service';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
    ngOnInit(): void {
        this.nav.hide()
    }

    constructor(private nav: NavbarFooterService) {

    }
    
}
