import style from './Modal.module.css';

function Modal(props) {

    // FUNCTIONS *********************************************************************************

    // If user click on the background, we close the modal popup
    const clickBackground = () => {

        props.setClosedModal(!props.closedModal);
    };

    // Prevent click on the popup itself for closing accidentely
    const preventDefaultClick = (e) => {

        e.stopPropagation();
    };

    // RENDERING *********************************************************************************

    return (

        <div style={props.closedModal ? { display: 'none' } : { style: 'block' }} id={style["modal"]} onClick={clickBackground} >
            <div onClick={preventDefaultClick}>
                <h2>Verifiez votre resultat</h2>
                <p>Saisissez votre code ci-dessous pour verifier votre resultat</p>
                <div>
                    <span className='errorModal'>{props.errorMessage}</span>
                </div>
                <input type="text" maxLength="22" placeholder="Saisissez votre code" onChange={props.setAccessCode} value={props.accessCode}/>
                <div>
                    <button onClick={() => props.setClosedModal(true)}>Annuler</button>
                    <button onClick={props.navToResult} disabled={props.accessCode.length !== 22}>Valider</button>
                </div>
            </div>
        </div>
    );

}

export default Modal;

