import { Component, OnInit } from '@angular/core';
import { PostService } from '../../_services/post-service';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-post-list',
    templateUrl: './post-list.component.html',
    styleUrls: ['./post-list.component.css']
})
export class PostListComponent implements OnInit {
    ngOnInit(): void {
        this.nav.show()

        this.page = 1;
        this.limit = 10;
        this.total = this.postsService.getTotal()
        this.getPosts();
    }
    public posts: IPost[]
    private total: number;
    private page: number;
    private limit: number;

    public ableToGoNextPage(): boolean{
        return this.page*this.limit < this.total;
    }

    public ableToGoPreviousPage() : boolean{
        return this.page > 1;
    }

    constructor(private postsService: PostService,
        private nav: NavbarFooterService
    ) {
        this.page = 1;
        this.limit = 10;
        this.total = this.postsService.getTotal()
        this.getPosts();
    }

    getPosts(): void {
        this.posts = this.postsService.getPosts(this.page, this.limit)
    }

    goToPrevious(): void {
        //console.log('Previous Button Clicked!');
        if (this.ableToGoPreviousPage()) {
            this.page--;
            this.getPosts()
        }
    }

    goToNext(): void {
        //console.log('Next Button Clicked!');
        if (this.ableToGoNextPage()) {
            this.page++;
            this.getPosts()
        }
    }
}
