import style from './Loading.module.css';
import wheel from './wheel.gif';

function Loading() {

    // RENDERING *********************************************************************************
    
    return(
        <section className={style.loading}>
            <div>
                <h1>¥€$!</h1>
                <h3>Récupération des informations...</h3>     
                <h3>Merci de patienter</h3>  
                <img src={wheel} alt="Loading wheel" id={style["spinLoad"]}/>             
            </div>   
        </section>   
    );
}

export default Loading;