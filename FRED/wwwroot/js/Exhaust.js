class Exhaust {
    constructor(exhausty, exhaustx) {
        this.y = exhausty;
        this.x = exhaustx;
        this.size = Math.floor(Math.random() * 30) + 3;
    }
    Move() {
        if (this.y <= 300) {
            this.y = 470;
            this.x = 500;
            this.size = Math.floor(Math.random() * 30) + 3;
        }
        else {
            this.y -= Math.floor(Math.random() * 3) + 1;
            this.x -= Math.floor(Math.random() * 5) + 1;
            this.size = Math.floor(Math.random() * 30) + 1;
        }
    }
}