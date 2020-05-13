import { Component, Inject, OnInit } from '@angular/core';
import { UserService } from '../../_services/user-service';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-users',
    templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit{
    ngOnInit(): void {
        this.nav.show()
    }
    private users: IUser[]
    private total: number;
    private page: number;
    private limit: number;

    public ableToGoNextPage(): boolean{
        return this.page*this.limit < this.total;
    }
    public ableToGoPreviousPage() : boolean{
        return this.page > 1;
    } 

    constructor(private userService: UserService,
        private nav: NavbarFooterService) {
        this.page = 1;
        this.limit = 10;
        this.getUsers();
    }

    getUsers(): void {
        this.users = this.userService.getUsers(this.page, this.limit)
        this.total = this.userService.getTotal()
    }

    goToPrevious(): void {
        //console.log('Previous Button Clicked!');
        if (this.ableToGoPreviousPage()) {
            this.page--;
            this.getUsers();
        }
    }

    goToNext(): void {
        //console.log('Next Button Clicked!');
        if (this.ableToGoNextPage()) {
            this.page++;
            this.getUsers();
        }
    }
}
